using System;
using NUnit.Framework;
using SharpTestsEx;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Feriados
{
    [TestFixture]
    public class FeriadoTests : InitializeNhibernate
    {
        [Test]
        public void PuedoGrabarUnFeriado()
        {
            Feriado feriado;
            using(var tx = Session.BeginTransaction())
            {
                feriado = new Feriado()
                          {
                              Nombre = "Paso a la Inmortalidad del General José de San Martín",
                              Fecha = new DateTime(2011, 08, 17),
                          };
                Session.Save(feriado);
                tx.Commit();
            }

            Session.Flush();
            Session.Evict(feriado);

            var db = Session.Get<Feriado>(feriado.Id);

            Assert.NotNull(db);
            db.Nombre.Should().Be(feriado.Nombre);
        }
    }
}