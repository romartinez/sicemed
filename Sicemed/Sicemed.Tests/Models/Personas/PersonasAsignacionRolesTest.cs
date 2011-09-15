using System;
using System.Linq;
using System.Security.Principal;
using NUnit.Framework;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Tests.Models.Personas
{
    [TestFixture]
    public class PersonasAsignacionRolesTest : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnUsuarioConUnSoloRol()
        {
            var persona = CrearPersonaValida();
            persona.AgregarRol(Profesional.Create("54654"));
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.AreEqual(1, personaDb.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioConVariosRoles()
        {
            var persona = CrearPersonaValida();
            persona.AgregarRol(Secretaria.Create(DateTime.Now));
            persona.AgregarRol(Profesional.Create("56465489"));
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);


            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.AreEqual(2, personaDb.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioSinRoles()
        {
            var persona = CrearPersonaValida();
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.IsNotNull(personaDb);
            Assert.AreEqual(0, personaDb.Roles.Count());
        }


        [Test]
        public void PuedoCrearUnaPersonaSimple()
        {
            var persona = CrearPersonaValida();            
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.IsNotNull(personaDb);
            Assert.AreEqual(0, personaDb.Roles.Count());            
        }

        [Test]
        public void PuedoTraerElTipoDePersonaConcreta()
        {
            var persona = CrearPersonaValida();
            var date = DateTime.Now;
            persona.AgregarRol(Secretaria.Create(date));
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.IsNotNull(personaDb);
            Assert.DoesNotThrow(() => personaDb.As<Secretaria>());
            //Le hago el tostring porque en la base se guarda sin los milliseconds en sqlite
            Assert.AreEqual(date.ToString(), personaDb.As<Secretaria>().FechaIngreso.ToString());
        }

        [Test]
        public void LanzaUnaExcepcionAlTraerUnaPersonaComoUnTipoQueNoEs()
        {
            var persona = CrearPersonaValida();
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.IsNotNull(personaDb);
            Assert.Throws<IdentityNotMappedException>(() => personaDb.As<Secretaria>());
        }

        [Test]
        public void PuedoConsutarSiUnaPersonaEsDeUnTipo()
        {
            var persona = CrearPersonaValida();
            persona.AgregarRol(Administrador.Create());
            MembershipService.CreateUser(persona, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(persona);

            var personaDb = Session.Get<Persona>(persona.Id);
            Assert.IsNotNull(personaDb);
            Assert.IsFalse(personaDb.IsInRole<Secretaria>());
            Assert.IsTrue(personaDb.IsInRole<Administrador>());
        }



    }
}