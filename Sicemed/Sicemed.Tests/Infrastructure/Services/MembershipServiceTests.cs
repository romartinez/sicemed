using NUnit.Framework;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Infrastructure.Services
{
    public class MembershipServiceTests : InitializeNhibernate
    {

        [Test]
        public void PuedoCrearUnUsuario()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            MailService.Verify(x => x.SendNewUserEmail(usuario));

            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.AreEqual("Walter", usuario2.Nombre);
        }

        [Test]
        public void PuedoLoguarme()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Session.Flush();
            Session.Evict(usuario);

            Usuario u2;
            MembershipService.Login("walter.poch@gmail.com", "testtest", out u2);
            Assert.AreEqual(usuario, u2);
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalPassword()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, MembershipService.Login("walter.poch@gmail.com", "te2sttest", out usuario));
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalUsuario()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.USER_NOT_FOUND, MembershipService.Login("wal333ter.poch@gmail.com", "testtest", out usuario));
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalUsuarioYMalPassword()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");


            Assert.AreEqual(MembershipStatus.USER_NOT_FOUND,
                MembershipService.Login("wal333ter.poch@gmail.com", "tesdfgttest", out usuario));
        }

        [Test]
        public void PuedoBloquearUnUsuario()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            MembershipService.LockUser("walter.poch@gmail.com", "Testing");


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsTrue(usuario2.Membership.IsLockedOut);
            Assert.AreEqual("Testing", usuario2.Membership.LockedOutReason);
        }

        [Test]
        public void PuedoBloquearYDesbloquearUnUsuario()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            MembershipService.LockUser("walter.poch@gmail.com", "Testing");

            MembershipService.UnlockUser("walter.poch@gmail.com");


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsFalse(usuario2.Membership.IsLockedOut);
            Assert.AreEqual("Testing", usuario2.Membership.LockedOutReason);
        }

        [Test]
        public void UnUsuarioSeBloqueaCon3IntentosDeLoginFallidos()
        {
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, MembershipService.Login("walter.poch@gmail.com", "345", out usuario));

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, MembershipService.Login("walter.poch@gmail.com", "345", out usuario));

            Assert.AreEqual(MembershipStatus.USER_LOCKED, MembershipService.Login("walter.poch@gmail.com", "345", out usuario));


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsTrue(usuario2.Membership.IsLockedOut);
            Assert.IsFalse(string.IsNullOrWhiteSpace(usuario2.Membership.LockedOutReason));

            Assert.AreEqual(MembershipStatus.USER_LOCKED, MembershipService.Login("walter.poch@gmail.com", "testtest", out usuario));
        }

        [Test]
        public void PuedoPedirUnPasswordReset()
        {
            var token = string.Empty;
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");


            MembershipService.RecoverPassword("walter.poch@gmail.com");
            token = usuario.Membership.PasswordResetToken;
            MailService.Verify(x => x.SendPasswordResetEmail(usuario, token));


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.AreEqual(token, usuario2.Membership.PasswordResetToken);
        }

        [Test]
        public void PuedoCambiarElPasswordYLoguearmeConElNuevo()
        {
            var token = string.Empty;
            var usuario = CrearUsuarioValido();

            MembershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            MembershipService.RecoverPassword("walter.poch@gmail.com");

            token = usuario.Membership.PasswordResetToken;
            MailService.Verify(x => x.SendPasswordResetEmail(usuario, token));

            MembershipService.ChangePassword("walter.poch@gmail.com", token, "walter2");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, MembershipService.Login("walter.poch@gmail.com", "testtest", out usuario));

            Assert.AreEqual(MembershipStatus.USER_FOUND, MembershipService.Login("walter.poch@gmail.com", "walter2", out usuario));
        }
    }
}