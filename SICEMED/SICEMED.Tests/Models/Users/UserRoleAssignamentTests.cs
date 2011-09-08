using System.Linq;
using NUnit.Framework;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Roles;

namespace Sicemed.Tests.Models.Users
{
    [TestFixture]
    public class UserRoleAssignamentTests : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnUsuarioConUnSoloRol()
        {
            var usuario = CrearUsuarioValido();
            usuario.AgregarRol(Rol.Profesional);
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);

            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre == "Walter").SingleOrDefault();
            Assert.AreEqual(1, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioConVariosRoles()
        {
            var usuario = CrearUsuarioValido();
            usuario.AgregarRol(Rol.Secretaria);
            usuario.AgregarRol(Rol.Profesional);
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);


            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre == "Walter").SingleOrDefault();
            Assert.AreEqual(2, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioSinRoles()
        {
            var usuario = CrearUsuarioValido();
            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);

            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre == "Walter").SingleOrDefault();
            Assert.IsNotNull(usuario2);
            Assert.AreEqual(0, usuario2.Roles.Count());
        }
    }
}