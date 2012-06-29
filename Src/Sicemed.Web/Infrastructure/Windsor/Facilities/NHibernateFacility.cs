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
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Sicemed.Web.Infrastructure.HttpModules;
using Sicemed.Web.Infrastructure.NHibernate;
using Sicemed.Web.Infrastructure.NHibernate.Events;
using Sicemed.Web.Models;

namespace SICEMED.Web.Infrastructure.Windsor.Facilities
{
    public class NHibernateFacility : AbstractFacility
    {
        protected override void Init()
        {
            var config = BuildDatabaseConfiguration();

            Kernel.Register(Component.For<ISessionFactory>()
                                           .UsingFactoryMethod(k => config.BuildSessionFactory()));

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
                                                  db.LogFormattedSql = true;
                                                  db.HqlToSqlSubstitutions = "true 1, false 0, yes 'Y', no 'N'";
                                              });
            
            //NOTE: Uso mapeo por XML, para fine-tunning, por problemas en los many-to-many
            //var mappings = GetHbmMappings();
            //mappings.ToList().ForEach(mp => configuration.AddDeserializedMapping(mp, null));

            configuration.AddAssembly(typeof(NHibernateFacility).Assembly);

            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            configuration.Properties[Environment.CurrentSessionContextClass]
                = typeof(LazySessionContext).AssemblyQualifiedName; 

            //var auditListener = new AuditEventListener();

            //configuration.SetListener(ListenerType.PostDelete, auditListener);
            //configuration.SetListener(ListenerType.PostInsert, auditListener);
            //configuration.SetListener(ListenerType.PostUpdate, auditListener);

			configuration.SetListener(ListenerType.Flush, new PostFlushFixEventListener());


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

            return mapper.CompileMappingForEach(typeof(Entity).Assembly.GetTypes());
        }
    }
}