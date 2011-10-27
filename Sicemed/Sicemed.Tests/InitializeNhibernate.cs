using System;
using System.Configuration;
using System.Data.SqlClient;
using EfficientlyLazy.Crypto;
using Moq;
using NHibernate;
using NHibernate.Context;
using NUnit.Framework;
using SICEMED.Web.Infrastructure.Windsor.Facilities;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;
using log4net;
using log4net.Config;
using log4net.Core;
using Configuration = NHibernate.Cfg.Configuration;
using Environment = NHibernate.Cfg.Environment;

namespace Sicemed.Tests
{
    public class InitializeNhibernate
    {
        protected ILog Log = LogManager.GetLogger(typeof(InitializeNhibernate));
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

                    _databaseConfiguration = NHibernateFacility.BuildDatabaseConfiguration();

                    _databaseConfiguration.Properties[Environment.CurrentSessionContextClass] =
                        typeof(ThreadStaticSessionContext).AssemblyQualifiedName;

                    var mappings = NHibernateFacility.GetHbmMappings();
                    mappings.WriteAllXmlMapping();
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
            var path = @"D:\Program Files\Microsoft SQL Server\MSSQL.2\MSSQL\Data\Sicemed_Snapshot.ss";
            //var path = @"C:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\Sicemed_Snapshot.ss";
            CreateDatabaseSnapshot("Sicemed_Snapshot", "SicemedTest", path);
            LogManager.GetRepository().Threshold = Level.Off;

            CurrentSessionContext.Bind(SessionFactory.OpenSession());
            
            var installer = new ApplicationInstaller();
            installer.SessionFactory = SessionFactory;

            installer.MembershipService = new MembershipService(SessionFactory,
                                                                new Mock<IMailSenderService>().Object,
                                                                new Mock<IFormAuthenticationStoreService>().Object);
            installer.Install(DatabaseConfiguration);

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
            RevertDatabaseFromSnapshot("SicemedTest", "Sicemed_Snapshot");
        }


        protected Persona CrearPersonaValida()
        {
            return new Persona { Nombre = "Walter", Apellido = "Poch" };
        }

        private void CreateDatabaseSnapshot(string snapshotName, string originalDb, string snapshotPath)
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cnn;
                        cmd.CommandTimeout = 1000;
                        cmd.CommandText = string.Format("USE master;");
                        cmd.CommandText += string.Format("CREATE DATABASE {0} ON ( NAME = {1}, FILENAME = '{2}' ) AS SNAPSHOT OF {1};", 
                            snapshotName, originalDb, snapshotPath);

                        cnn.Open();

                        Log.Info("CREATE SNAPSHOT: " + cmd.ExecuteNonQuery().ToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex);
                    }
                }
                cnn.Close();
            }
        }

        private void RevertDatabaseFromSnapshot(string databaseName, string snapshotName)
        {
            using (var cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    try
                    {
                        cmd.Connection = cnn;
                        cmd.CommandTimeout = 1000;
                        cmd.CommandText = string.Format("USE master;");
                        cmd.CommandText += string.Format("ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", databaseName);
                        cmd.CommandText += string.Format("RESTORE DATABASE {0} FROM DATABASE_SNAPSHOT = '{1}';", databaseName, snapshotName);
                        cmd.CommandText += string.Format("DROP DATABASE {0};", snapshotName);
                        cmd.CommandText += string.Format("ALTER DATABASE {0} SET MULTI_USER;", databaseName);

                        cnn.Open();
                        Log.Info("REVERT SNAPSHOT: " + cmd.ExecuteNonQuery().ToString());
                    }
                    catch (Exception ex)
                    {
                        Log.Fatal(ex);
                    }
                }
            }
        }
    }
}