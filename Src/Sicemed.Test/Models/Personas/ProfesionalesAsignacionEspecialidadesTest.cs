using System;
using NUnit.Framework;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Tests.Models.Personas
{
    [TestFixture]
    public class ProfesionalesAsignacionEspecialidadesTest : InitializeNhibernate
    {
        [Test]
        public void PuedoAsignarUnaEspecialidadAUnProfesional()
        {
            var persona = CrearPersonaValida();
            persona.AgregarRol(Profesional.Create("AAAsd"));
            MembershipService.CreateUser(persona, "persona@gmail.com", "testtest");

            var session = SessionFactory.GetCurrentSession();
            session.Flush();


            var especialidad = new Especialidad() { Nombre = "Traumatologo" };
            session.Save(especialidad);
            session.Flush();

            Console.WriteLine(@"Agregando especialidad..");
            var profesional = persona.As<Profesional>();
            profesional.AgregarEspecialidad(especialidad);
            session.SaveOrUpdate(profesional);
            session.Flush();

            var personaDb = session.Get<Persona>(persona.Id);
            var profesionalDb = personaDb.As<Profesional>();

            Assert.NotNull(personaDb);
            Assert.AreEqual(1, profesionalDb.Especialidades.Count);
        }        
        
        [Test]
        public void PuedoAsignarUnaEspecialidadesAUnProfesional()
        {
            var session = SessionFactory.GetCurrentSession();
            var profesional = ApplicationInstaller.PersonaProfesionalBernardoClinico.As<Profesional>();
            profesional.AgregarEspecialidad(ApplicationInstaller.EspecialidadClinico);

            session.Update(ApplicationInstaller.PersonaProfesionalBernardoClinico);

            var personaDb = session.Get<Persona>(ApplicationInstaller.PersonaProfesionalBernardoClinico.Id);
            var profesionalDb = personaDb.As<Profesional>();            

            Assert.NotNull(personaDb);
            Assert.AreEqual(1, profesionalDb.Especialidades.Count);
        }
    }
}