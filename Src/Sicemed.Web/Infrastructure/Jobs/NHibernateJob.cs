using System;
using NHibernate;
using NHibernate.Cfg;
using SICEMED.Web.Infrastructure.Windsor.Facilities;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public abstract class NHibernateJob : SimpleJob
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

        protected override void Run()
        {
            using (var session = SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                try
                {
                    Run(session);                    
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    if (Log.IsFatalEnabled) Log.Fatal(ex);
                    tx.Rollback();
                }
            }            
        }

        protected abstract void Run(ISession session);
    }
}