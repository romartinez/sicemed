using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Sicemed.Web.Plumbing.Facilities;

namespace Sicemed.Tests.Persistence {
    [TestFixture]
    public class InitializeNhbernate {
        [Test]
        public void CanCreateSchema() {
            var config = new PersistenceFacility().BuildDatabaseConfiguration();
            var exporter = new SchemaExport(config);
            exporter.Create(true, true);
        }
    }
}
