using NUnit.Framework;
using SharpTestsEx;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Localidades
{
    [TestFixture]
    public class LocalidadesTests : InitializeNhibernate
    {
         
        [Test]
        public void PuedoAgregarUnaLocalidadAUnaProvincia()
        {
            var provincia = new Provincia() {Nombre = "Buenos Aires"};
            using(var tx = Session.BeginTransaction())
            {
                Session.Save(provincia);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(provincia);

            var localidad = new Localidad() {Nombre = "Pergamino"};
            using(var tx = Session.BeginTransaction())
            {
                var p2 = Session.Get<Provincia>(provincia.Id);
                p2.AgregarLocalidad(localidad);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(localidad);

            var l2 = Session.Get<Localidad>(localidad.Id);

            Assert.NotNull(l2);
            l2.Nombre.Should().Be(localidad.Nombre);
        }
    }
}