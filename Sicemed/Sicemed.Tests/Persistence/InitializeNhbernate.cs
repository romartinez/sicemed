using System;
using System.Configuration;
using Castle.Core.Internal;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers.Builders;
using NHibernate.ByteCode.Castle;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;
using Configuration = NHibernate.Cfg.Configuration;

namespace Sicemed.Tests.Persistence {
    [TestFixture]
    public class InitializeNhbernate {
        public Configuration BuildDatabaseConfiguration() {
            return Fluently.Configure()
                .Database(SetupDatabase)
                .Mappings(m => m.AutoMappings.Add(CreateMappingModel()))
                .ExposeConfiguration(ConfigurePersistence)
                .BuildConfiguration();
        }

        protected virtual AutoPersistenceModel CreateMappingModel() {
            var m = AutoMap.Assembly(typeof(EntityBase).Assembly)
                .Where(IsDomainEntity)
                .OverrideAll(ShouldIgnoreProperty)
                .IgnoreBase<EntityBase>()
                .Conventions.Add(new ComponentConventionBuilder().Always(x => x.Insert()));
            return m;
        }

        protected virtual IPersistenceConfigurer SetupDatabase() {
            Console.WriteLine("Connection String: {0}", ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            return MsSqlConfiguration.MsSql2008
                .UseOuterJoin()
                .ProxyFactoryFactory(typeof(ProxyFactoryFactory))
                .ConnectionString(x => x.FromConnectionStringWithKey("ApplicationServices"))
                .ShowSql();
        }

        protected virtual void ConfigurePersistence(Configuration config) {
            SchemaMetadataUpdater.QuoteTableAndColumns(config);
        }

        protected virtual bool IsDomainEntity(Type t) {
            return typeof(EntityBase).IsAssignableFrom(t);
        }

        private void ShouldIgnoreProperty(IPropertyIgnorer property) {
            property.IgnoreProperties(p => p.MemberInfo.HasAttribute<DoNotMapAttribute>());
        }

        
        [Test]
        public void CanCreateSchema() {
            var config = BuildDatabaseConfiguration();
            var exporter = new SchemaExport(config);
            exporter.Create(true, true);
        }
    }
}
