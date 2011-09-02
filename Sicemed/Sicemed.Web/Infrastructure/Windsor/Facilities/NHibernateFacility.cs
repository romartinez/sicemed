﻿using System.Collections.Generic;
using System.Data;
using System.Web;
using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.Cfg;
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

            configuration.AddAssembly(typeof(Entity).Assembly);
            
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration);

            configuration.Properties[Environment.CurrentSessionContextClass]
                = typeof(LazySessionContext).AssemblyQualifiedName;


            return configuration;
        }
    }
}