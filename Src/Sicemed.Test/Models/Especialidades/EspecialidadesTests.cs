using NHibernate.Exceptions;
using NUnit.Framework;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Especialidades
{
    [TestFixture]
    public class EspecialidadesTests : InitializeNhibernate
    {
         
        [Test]
        public void PuedoCrearUnaEspecialidad()
        {
            var especialidad = new Especialidad() {Nombre = "Traumatologo"};

            var session = SessionFactory.GetCurrentSession();

            session.Save(especialidad);

            session.Flush();
            session.Evict(especialidad);

            var especialidadDb = session.Get<Especialidad>(especialidad.Id);

            Assert.NotNull(especialidadDb);
            Assert.AreEqual(especialidad.Id, especialidadDb.Id);
            Assert.AreEqual(especialidad.Nombre, especialidadDb.Nombre);
        }

        [Test]
        public void NoPuedoCrearUnaEspecialidadConNombreDuplicado()
        {
            var especialidad = new Especialidad() {Nombre = "Traumatologo"};
            var especialidadDuplicada = new Especialidad() {Nombre = "Traumatologo"};

            var session = SessionFactory.GetCurrentSession();

            session.Save(especialidad);
            session.Save(especialidadDuplicada);

            Assert.Throws<GenericADOException>(session.Flush);            
        }
    }
}