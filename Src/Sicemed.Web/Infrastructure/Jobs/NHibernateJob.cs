using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using WebBackgrounder;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public abstract class NHibernateJob : Job
    {        
        private static Configuration _databaseConfiguration;
        private static ISessionFactory _sessionFactory;
        
        public static Configuration DatabaseConfiguration
        {
            get
            {
                if (_databaseConfiguration == null)
                {
                    _databaseConfiguration = NHibernateFacility.BuildDatabaseConfiguration();

                    _databaseConfiguration.Properties[global::NHibernate.Cfg.Environment.CurrentSessionContextClass] =
                        typeof(ThreadStaticSessionContext).AssemblyQualifiedName;
                }

                return _databaseConfiguration;
            }
        }

        protected static ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? (_sessionFactory = DatabaseConfiguration.BuildSessionFactory()); }
        }

        protected static ISession Session
        {
            get { return _sessionFactory.GetCurrentSession(); }
        }

        protected NHibernateJob(string name, TimeSpan interval, TimeSpan timeout) 
            : base(name, interval, timeout){}

        protected NHibernateJob(string name, TimeSpan interval) 
            : base(name, interval){}
    }
}