using System;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Sicemed.Web.Models;
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


        [TestFixtureSetUp]
        public void CanCreateSchema()
        {
            var config = BuildDatabaseConfiguration();
            var exporter = new SchemaExport(config);
            exporter.Create(true, true);
        }


        [Test]
        public void InsertTestData()
        {
            var config = BuildDatabaseConfiguration();
            var factory = config.BuildSessionFactory();
            using(var session = factory.OpenSession())
            using(var tx = session.BeginTransaction())
            {
                var c = new Calendario() {Anio = DateTime.Now, Nombre = DateTime.Now.ToShortDateString()};
                c.AddFeriado(new Feriado() { Nombre = "Feriado 1", FechaOriginal = new DateTime(2011,05,25)});
                session.Save(c);
                tx.Commit();
            }
        }
    }
}