using System.Data;
using System.Linq;
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
using Sicemed.Web.Models;
using log4net;
using log4net.Config;
using log4net.Core;

namespace Sicemed.Tests
{
    public class InitializeNhibernate
    {
        private const bool USAR_SQL_LITE = true;

        private static Configuration _databaseConfiguration;

        private static ISessionFactory _sessionFactory;
        private Mock<IMailSenderService> _mailService;
        private MembershipService _membershipService;

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

                    if (USAR_SQL_LITE)
                    {
                        _databaseConfiguration.DataBaseIntegration(db =>
                                                                   {
                                                                       //http://stackoverflow.com/questions/189280/problem-using-sqlite-memory-with-nhibernate
                                                                       db.Dialect<SQLiteDialect>();
                                                                       db.Driver<SQLite20Driver>();
                                                                       db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                                       db.LogSqlInConsole = true;
                                                                       db.ConnectionString =
                                                                           "Data Source=:memory:;Version=3;New=True;";
                                                                       db.AutoCommentSql = true;
                                                                       db.ConnectionReleaseMode =
                                                                           ConnectionReleaseMode.OnClose;
                                                                       db.HqlToSqlSubstitutions =
                                                                           "true 1, false 0, yes 'Y', no 'N'";
                                                                   });
                    } else
                    {
                        _databaseConfiguration.DataBaseIntegration(db =>
                                                                   {
                                                                       db.Dialect<MsSql2008Dialect>();
                                                                       db.Driver<SqlClientDriver>();
                                                                       db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                                                                       db.IsolationLevel = IsolationLevel.ReadCommitted;
                                                                       db.ConnectionStringName = "ApplicationServices";
                                                                       db.Timeout = 10;
                                                                       db.LogSqlInConsole = true;
                                                                       db.AutoCommentSql = true;
                                                                       db.HqlToSqlSubstitutions =
                                                                           "true 1, false 0, yes 'Y', no 'N'";
                                                                   });
                    }

                    _databaseConfiguration.Properties[Environment.CurrentSessionContextClass] =
                        typeof (ThreadStaticSessionContext).AssemblyQualifiedName;

                    var mappings = NHibernateFacility.GetHbmMappings();

                    mappings.WriteAllXmlMapping();

                    mappings.ToList().ForEach(mp => _databaseConfiguration.AddDeserializedMapping(mp, null));

                    SchemaMetadataUpdater.QuoteTableAndColumns(_databaseConfiguration);
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

        protected Mock<IMailSenderService> MailService
        {
            get { return _mailService; }
        }

        protected IMembershipService MembershipService
        {
            get { return _membershipService; }
        }


        [SetUp]
        public void Setup()
        {
            var session = SessionFactory.OpenSession();
            CurrentSessionContext.Bind(session);
            LogManager.GetRepository().Threshold = Level.Off;

            var installer = new ApplicationInstaller();
            installer.SessionFactory = SessionFactory;

            installer.MembershipService = new MembershipService(SessionFactory,
                                                                new Mock<IMailSenderService>().Object,
                                                                new Mock<IFormAuthenticationStoreService>().Object);

            installer.Install(DatabaseConfiguration);

            //After the installation bind the session, again
            CurrentSessionContext.Bind(session);

            LogManager.GetRepository().Threshold = Level.Debug;

            new RijndaelEngine("WAL");
            _mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();
            _membershipService = new MembershipService(SessionFactory,
                                                       _mailService.Object,
                                                       formsService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            CurrentSessionContext.Unbind(SessionFactory);
        }


        protected Persona CrearPersonaValida()
        {
            return new Persona {Nombre = "Walter", Apellido = "Poch"};
        }
    }
}