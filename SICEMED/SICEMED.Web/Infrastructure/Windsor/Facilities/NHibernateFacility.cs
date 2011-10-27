using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Impl;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.HttpModules;
using Sicemed.Web.Infrastructure.NHibernate.Events;
using Sicemed.Web.Infrastructure.Providers.Session;
using Sicemed.Web.Models;

namespace SICEMED.Web.Infrastructure.Windsor.Facilities
{
    public class NHibernateFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(Component.For<ISessionFactory>()
                .UsingFactoryMethod(k =>
                {
                    var sessionFactory = BuildDatabaseConfiguration().BuildSessionFactory();
                    ((SessionFactoryImpl)sessionFactory).EventListeners.
                        SaveOrUpdateEventListeners = new[] { new SaveOrUpdateEventListener() };

                    return sessionFactory;
                }).LifeStyle.Singleton);

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
            configuration.CollectionTypeFactory<Net4CollectionTypeFactory>();
            configuration.DataBaseIntegration(db =>
                                              {
                                                  db.Dialect<MsSql2008Dialect>();
                                                  db.Driver<SqlClientDriver>();
                                                  db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                  db.IsolationLevel = IsolationLevel.ReadCommitted;
                                                  db.ConnectionStringName = "ApplicationServices";
                                                  db.Timeout = 10;
                                                  db.LogFormattedSql = true;
                                                  db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                                              });

            var mappings = GetHbmMappings();

            mappings.ToList().ForEach(mp => configuration.AddDeserializedMapping(mp, null));

            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            configuration.Properties[Environment.CurrentSessionContextClass]
                = typeof(LazySessionContext).AssemblyQualifiedName;


            return configuration;
        }

        public static IEnumerable<HbmMapping> GetHbmMappings()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(typeof(Entity).Assembly.GetTypes());

            mapper.BeforeMapProperty += (mi, propertyPath, map) =>
                                        {
                                            if (propertyPath.PreviousPath != null
                                                && mi.IsComponent(propertyPath.LocalMember.DeclaringType))
                                                map.Column(propertyPath.ToColumnName());
                                        };

            //mapper.BeforeMapClass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());
            //mapper.BeforeMapJoinedSubclass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());
            //mapper.BeforeMapUnionSubclass += (mi, t, map) => map.Table(t.Name.ToLowerInvariant());

            //mapper.BeforeMapBag += (mi, propPath, map) =>
            //{
            //    map.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
            //    map.BatchSize(10);
            //};

            //mapper.BeforeMapSet += (mi, propPath, map) =>
            //{
            //    map.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
            //    map.BatchSize(10);
            //};

            return mapper.CompileMappingForEach(typeof(Entity).Assembly.GetTypes());
        }
    }
}