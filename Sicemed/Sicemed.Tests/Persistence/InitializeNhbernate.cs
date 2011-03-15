using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Sicemed.Web.Plumbing.Facilities;


namespace Sicemed.Tests.Persistence
{
    [TestFixture]
    public class InitializeNhbernate
    {
        public Configuration BuildDatabaseConfiguration()
        {
            return PersistenceFacility.BuildDatabaseConfiguration();
        }


        [Test]
        public void CanCreateSchema()
        {
            var config = BuildDatabaseConfiguration();
            var exporter = new SchemaExport(config);
            exporter.Create(true, true);
        }
    }
}