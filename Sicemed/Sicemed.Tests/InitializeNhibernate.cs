using System;
using System.Collections.Generic;
using System.Linq;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Patterns;
using ConfOrm.Shop.CoolNaming;
using ConfOrm.Shop.InflectorNaming;
using ConfOrm.Shop.Inflectors;
using ConfOrm.Shop.Packs;
using ConfOrm.Shop.Patterns;
using EfficientlyLazy.Crypto;
using Moq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using log4net;
using log4net.Config;
using log4net.Core;
using Configuration = NHibernate.Cfg.Configuration;
using Environment = NHibernate.Cfg.Environment;

namespace Sicemed.Tests
{
    public class InitializeNhibernate
    {
        private static Configuration _databaseConfiguration;

        private static ISessionFactory _sessionFactory;

        public static Configuration DatabaseConfiguration
        {
            get
            {
                if (_databaseConfiguration == null)
                {
                    //Lo configuro acá asi no veo los primeros inserts :D
                    XmlConfigurator.Configure();

                    _databaseConfiguration = new Configuration();
                    _databaseConfiguration.CollectionTypeFactory<Net4CollectionTypeFactory>();            


                    _databaseConfiguration.DataBaseIntegration(db =>
                                                               {
                                                                   //http://stackoverflow.com/questions/189280/problem-using-sqlite-memory-with-nhibernate
                                                                   db.Dialect<SQLiteDialect>();
                                                                   db.Driver<SQLite20Driver>();
                                                                   db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                                   //db.LogFormatedSql = true;
                                                                   //db.LogSqlInConsole = true;
                                                                   db.ConnectionString =
                                                                       "Data Source=:memory:;Version=3;New=True;";
                                                                   //db.AutoCommentSql = true;
                                                                   db.ConnectionReleaseMode =
                                                                       ConnectionReleaseMode.OnClose;
                                                                   db.HqlToSqlSubstitutions =
                                                                       "true 1, false 0, yes 'Y', no 'N'";
                                                               });

                    //_databaseConfiguration.DataBaseIntegration(db =>
                    //{
                    //    db.Dialect<MsSql2008Dialect>();
                    //    db.Driver<SqlClientDriver>();
                    //    db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    //    db.IsolationLevel = IsolationLevel.ReadCommitted;
                    //    db.ConnectionStringName = "ApplicationServices";
                    //    db.Timeout = 10;
                    //    db.LogFormatedSql = true;
                    //    db.LogSqlInConsole = true;
                    //    db.AutoCommentSql = true;
                    //    db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                    //});

                    var mappings = GetMappings();
                    NHibernateMappingsExtensions.WriteAllXmlMapping(mappings);
                    _databaseConfiguration.AddAssembly(typeof (Entity).Assembly);
                    SchemaMetadataUpdater.QuoteTableAndColumns(_databaseConfiguration);
                    
                    _databaseConfiguration.Properties[Environment.CurrentSessionContextClass] =
                        typeof (ThreadStaticSessionContext).AssemblyQualifiedName;
                }

                return _databaseConfiguration;
            }
        }

        public static IEnumerable<HbmMapping> GetMappings()
        {
            var orm = new ObjectRelationalMapper();

            // Remove one of not required patterns
            orm.Patterns.ManyToOneRelations.Remove(
                orm.Patterns.ManyToOneRelations.Single(p => p.GetType() == typeof(OneToOneUnidirectionalToManyToOnePattern)));


            // Creation of inflector adding some special class name translation
            var inflector = new SpanishInflector();

            IPatternsAppliersHolder patternsAppliers =
                (new SafePropertyAccessorPack())
                    .Merge(new OneToOneRelationPack(orm))
                    .Merge(new BidirectionalManyToManyRelationPack(orm))
                    .Merge(new BidirectionalOneToManyRelationPack(orm))
                    .Merge(new DiscriminatorValueAsClassNamePack(orm))
                    .Merge(new CoolTablesAndColumnsNamingPack(orm))
                    .Merge(new BidirectionalOneToManyCascadeApplier(orm))
                    .Merge(new UnidirectionalOneToOneUniqueCascadeApplier(orm))
                    .Merge(new PolymorphismBidirectionalOneToManyCascadeApplier(orm))
                    .Merge(new TablePerClassPack())
                    .Merge(new EnumAsStringPack())
                    .Merge(new PluralizedTablesPack(orm, inflector)) // <=== 
                    .Merge(new ListIndexAsPropertyPosColumnNameApplier());

            orm.Patterns.PoidStrategies.Add(new HighLowPoidPattern());
            orm.Patterns.Bags.Add(new UseSetWhenGenericCollectionPattern());
            orm.Patterns.Sets.Add(new UseSetWhenGenericCollectionPattern());

            var entities = new List<Type>();
            entities.AddRange(typeof(Entity).Assembly.GetTypes().Where(t => typeof(Entity).IsAssignableFrom(t) && !(t.GetType() == typeof(Entity))));
            entities.Add(typeof(Parametro));
            entities.Add(typeof(Rol));

            orm.TablePerClass(entities);
            orm.TablePerClass<Parametro>();

            orm.TablePerClass<Rol>();

            var mapper = new Mapper(orm, patternsAppliers);
            //mapper.AddPropertyPattern(p => 
            //    ConfOrm.TypeExtensions.GetPropertyOrFieldType(p).Equals(typeof(Rol)), 
            //    mp => mp.Type<EnumerationType<Rol>>()
            //);


            mapper.Class<Parametro>(m =>
                                    {
                                        m.Id(p => p.Key);
                                        m.Property("_value", x => x.Access(Accessor.Field));
                                    });
            orm.ExcludeProperty<Parametro>(p => p.Key);

            return mapper.CompileMappingForEach(entities);
        }


        protected static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = DatabaseConfiguration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }

        protected ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        [SetUp]
        public void Setup()
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            LogManager.GetRepository().Threshold = Level.Off;

            var installer = new ApplicationInstaller();
            installer.SessionFactory = SessionFactory;

            installer.MembershipService = new MembershipService(SessionFactory, new RijndaelEngine("WAL"),
                                                                new Mock<IMailSenderService>().Object,
                                                                new Mock<IFormAuthenticationStoreService>().Object);
            
            installer.Install(DatabaseConfiguration);
            
            //After the installation bind the session, again
            CurrentSessionContext.Bind(session);

            LogManager.GetRepository().Threshold = Level.Debug;
        }

        [TearDown]
        public void TearDown()
        {
            CurrentSessionContext.Unbind(SessionFactory);
        }
    }
}