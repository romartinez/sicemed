using System.Collections.Generic;
using System.Data;
using System.Web;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.HttpModules;
using Sicemed.Web.Infrastructure.NHibernate.Events;
using Sicemed.Web.Models;

namespace SICEMED.Web.Infrastructure.Windsor.Facilities
{
    public class NHibernateFacility : AbstractFacility
    {
        protected override void Init()
        {
            var config = BuildDatabaseConfiguration();

            Kernel.Register(
                Component.For<ISessionFactory>()
                    .UsingFactoryMethod(k =>
                    {                                            
                        var sf = config.BuildSessionFactory();
                        HttpContext.Current.Application[NHibernateSessionModule.NH_SESSION_FACTORY_KEY] = sf;
                        return sf;
                    }));
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
                                                  db.LogFormattedSql = true;
                                                  db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                                              });

            //var mappings = GetHbmMappings();

            //mappings.ToList().ForEach(mp => configuration.AddDeserializedMapping(mp, null));

            configuration.AddAssembly(typeof(NHibernateFacility).Assembly);

            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            configuration.Properties[Environment.CurrentSessionContextClass] =
                typeof(WebSessionContext).AssemblyQualifiedName;

            var auditListener = new AuditEventListener();

            configuration.SetListener(ListenerType.PostDelete, auditListener);
            configuration.SetListener(ListenerType.PostInsert, auditListener);
            configuration.SetListener(ListenerType.PostUpdate, auditListener);

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