using System;
using System.Linq;
using NUnit.Framework;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Tests.Models.Users
{
    [TestFixture]
    public class UserRoleAssignamentTests : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnUsuarioConUnSoloRol()
        {
            var usuario = CrearPersonaValida();
            usuario.AgregarRol(new Profesional());
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);

            var usuario2 = Session.QueryOver<Persona>().Where(u => u.Nombre == "Walter" && u.Apellido == "Poch").SingleOrDefault();
            Assert.AreEqual(1, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioConVariosRoles()
        {
            var usuario = CrearPersonaValida();
            usuario.AgregarRol(new Secretaria(DateTime.Now));
            usuario.AgregarRol(new Profesional());
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);


            var usuario2 = Session.QueryOver<Persona>().Where(u => u.Nombre == "Walter" && u.Apellido == "Poch").SingleOrDefault();
            Assert.AreEqual(2, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioSinRoles()
        {
            var usuario = CrearPersonaValida();
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);

            var usuario2 = Session.QueryOver<Persona>().Where(u => u.Nombre == "Walter" && u.Apellido == "Poch").SingleOrDefault();
            Assert.IsNotNull(usuario2);
            Assert.AreEqual(0, usuario2.Roles.Count());
        }
    }
}