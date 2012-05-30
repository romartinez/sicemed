using System.Linq;
using NUnit.Framework;
using SharpTestsEx;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Provincias
{
    [TestFixture]
    public class ProvinciasTests : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnaProvincia()
        {
            Provincia provincia;
            using (var tx = Session.BeginTransaction())
            {
                provincia = new Provincia {Nombre = "Buenos Aires"};
                Session.Save(provincia);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(provincia);

            var db = Session.Get<Provincia>(provincia.Id);

            Assert.NotNull(db);
            db.Nombre.Should().Be(provincia.Nombre);
        }

        [Test]
        public void PuedoCrearUnaProvinciaConLocalidades()
        {
            Provincia provincia;
            using (var tx = Session.BeginTransaction())
            {
                provincia = new Provincia {Nombre = "Buenos Aires"};

                provincia.AgregarLocalidad(new Localidad {CodigoPostal = "2000", Nombre = "Pergamino"});
                provincia.AgregarLocalidad(new Localidad {CodigoPostal = "2700", Nombre = "Arrecifes"});

                Session.Save(provincia);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(provincia);

            var db = Session.Get<Provincia>(provincia.Id);

            Assert.NotNull(db);
            db.Nombre.Should().Be(provincia.Nombre);
            db.Localidades.Count().Should().Be(2);
        }
    }
}