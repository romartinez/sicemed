using EfficientlyLazy.Crypto;
using Moq;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using log4net;
using log4net.Config;
using log4net.Core;

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

                    //_databaseConfiguration.Proxy(p =>
                    //                             {
                    //                                 p.Validation = true;
                    //                                 p.ProxyFactoryFactory<ProxyFactoryFactory>();
                    //                             });

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

                    var mappings = NHibernateFacility.GetMapping();
                    new[] { mappings }.WriteAllXmlMapping();
                    _databaseConfiguration.AddDeserializedMapping(mappings, "Conform");
                    //_databaseConfiguration.AddAssembly(typeof (Usuario).Assembly);
                    SchemaMetadataUpdater.QuoteTableAndColumns(_databaseConfiguration);
                    
                    _databaseConfiguration.Properties[Environment.CurrentSessionContextClass] =
                        typeof (ThreadStaticSessionContext).AssemblyQualifiedName;
                }

                return _databaseConfiguration;
            }
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