using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using ConfOrm;
using ConfOrm.NH;
using ConfOrm.Patterns;
using ConfOrm.Shop.CoolNaming;
using ConfOrm.Shop.InflectorNaming;
using ConfOrm.Shop.Inflectors;
using ConfOrm.Shop.Packs;
using ConfOrm.Shop.Patterns;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.HttpModules;
using Sicemed.Web.Infrastructure.Providers.Session;
using Sicemed.Web.Models;
using Environment = NHibernate.Cfg.Environment;

namespace SICEMED.Web.Infrastructure.Windsor.Facilities
{
    public class NHibernateFacility : AbstractFacility
    {
        protected override void Init()
        {

            Kernel.Register(Component.For<ISessionFactory>()
                                           .UsingFactoryMethod(k => BuildDatabaseConfiguration().BuildSessionFactory()));

            Kernel.Register(Component.For<NHibernateSessionModule>());

            Kernel.Register(Component.For<ISessionFactoryProvider>().AsFactory());

            Kernel.Register(Component.For<IEnumerable<ISessionFactory>>()
                                        .UsingFactoryMethod(k => k.ResolveAll<ISessionFactory>()));

            HttpContext.Current.Application[SessionFactoryProvider.Key]
                            = Kernel.Resolve<ISessionFactoryProvider>();

        }

        public static Configuration BuildDatabaseConfiguration()
        {
            var configuration = new Configuration();
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2008Dialect>();
                db.Driver<SqlClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionStringName = "ApplicationServices";
                db.Timeout = 10;
                db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
            });
            var mappings = GetMapping();

            configuration.AddDeserializedMapping(mappings, "Sicemed_Domain");
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            configuration.Properties[Environment.CurrentSessionContextClass]
                = typeof(LazySessionContext).AssemblyQualifiedName;

            configuration.Properties[Environment.CollectionTypeFactoryClass]
                = typeof(Net4CollectionTypeFactory).AssemblyQualifiedName;

            return configuration;
        }


        public static HbmMapping GetMapping()
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
                            .Merge(new TablePerClassPack())
                            .Merge(new EnumAsStringPack())
                            .Merge(new PluralizedTablesPack(orm, inflector)) // <=== 
                            .Merge(new ListIndexAsPropertyPosColumnNameApplier());

            orm.Patterns.PoidStrategies.Add(new HighLowPoidPattern());
            orm.Patterns.Sets.Add(new UseSetWhenGenericCollectionPattern());

            var entities = new List<Type>();
            entities.AddRange(typeof(Entity).Assembly.GetTypes().Where(t => typeof(Entity).IsAssignableFrom(t) && !(t.GetType() == typeof(Entity))));
            var mapper = new Mapper(orm, patternsAppliers);
            orm.TablePerClass(entities);
            return mapper.CompileMappingFor(entities);
        }

    }
}