using System.Linq;
using NUnit.Framework;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Models.Users
{
    [TestFixture]
    public class UserRoleAssignamentTests : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnUsuarioConUnSoloRol()
        {
            using (var tx = Session.BeginTransaction())
            {
                var usuario = new Usuario { Nombre= "Walter"};
                usuario.AgregarRol(Rol.Profesional);
                Session.Save(usuario);
                tx.Commit();
            }


            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre== "Walter").SingleOrDefault();
            Assert.AreEqual(1, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioConVariosRoles()
        {
            using (var tx = Session.BeginTransaction())
            {
                var usuario = new Usuario { Nombre= "Walter"};
                usuario.AgregarRol(Rol.Secretaria);
                usuario.AgregarRol(Rol.Profesional);
                Session.Save(usuario);
                tx.Commit();
            }


            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre== "Walter").SingleOrDefault();
            Assert.AreEqual(2, usuario2.Roles.Count());
        }

        [Test]
        public void PuedoCrearUnUsuarioSinRoles()
        {
            using (var tx = Session.BeginTransaction())
            {
                var usuario = new Usuario { Nombre= "Walter"};
                Session.Save(usuario);
                tx.Commit();
            }

            var usuario2 = Session.QueryOver<Usuario>().Where(u => u.Nombre== "Walter").SingleOrDefault();
            Assert.IsNotNull(usuario2);
            Assert.AreEqual(0, usuario2.Roles.Count());
        }
    }
}