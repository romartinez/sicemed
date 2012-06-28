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
                }

                return _databaseConfiguration;
            }
        }

        protected static ISessionFactory SessionFactory
        {
            get
            {
                if(_sessionFactory == null) 
                    _sessionFactory = DatabaseConfiguration.BuildSessionFactory();

                return _sessionFactory;
            }
        }

        protected NHibernateJob(string name, TimeSpan interval, TimeSpan timeout) 
            : base(name, interval, timeout){}

        protected NHibernateJob(string name, TimeSpan interval) 
            : base(name, interval){}
    }
}