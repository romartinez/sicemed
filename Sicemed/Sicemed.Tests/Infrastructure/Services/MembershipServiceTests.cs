using EfficientlyLazy.Crypto;
using Moq;
using NUnit.Framework;
using Sicemed.Web.Infrastructure.Enums;
using Sicemed.Web.Infrastructure.Services;
using Sicemed.Web.Models;

namespace Sicemed.Tests.Infrastructure.Services
{
    public class MembershipServiceTests : InitializeNhibernate
    {
        [Test]
        public void PuedoCrearUnUsuario()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            mailService.Verify(x => x.SendNewUserEmail(usuario));

            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.AreEqual("Walter", usuario2.Nombre);
        }

        [Test]
        public void PuedoLoguarme()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Usuario u2;
            membershipService.Login("walter.poch@gmail.com", "testtest", out u2);
            Assert.AreEqual(usuario, u2);
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalPassword()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, membershipService.Login("walter.poch@gmail.com", "te2sttest", out usuario));
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalUsuario()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.USER_NOT_FOUND, membershipService.Login("wal333ter.poch@gmail.com", "testtest", out usuario));
        }

        [Test]
        public void NoPuedoLoguearmeConUnMalUsuarioYMalPassword()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");


            Assert.AreEqual(MembershipStatus.USER_NOT_FOUND,
                membershipService.Login("wal333ter.poch@gmail.com", "tesdfgttest", out usuario));
        }

        [Test]
        public void PuedoBloquearUnUsuario()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            membershipService.LockUser("walter.poch@gmail.com", "Testing");


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsTrue(usuario2.Membership.IsLockedOut);
            Assert.AreEqual("Testing", usuario2.Membership.LockedOutReason);
        }

        [Test]
        public void PuedoBloquearYDesbloquearUnUsuario()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            membershipService.LockUser("walter.poch@gmail.com", "Testing");

            membershipService.UnlockUser("walter.poch@gmail.com");


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsFalse(usuario2.Membership.IsLockedOut);
            Assert.AreEqual("Testing", usuario2.Membership.LockedOutReason);
        }

        [Test]
        public void UnUsuarioSeBloqueaCon3IntentosDeLoginFallidos()
        {
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, membershipService.Login("walter.poch@gmail.com", "345", out usuario));

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, membershipService.Login("walter.poch@gmail.com", "345", out usuario));

            Assert.AreEqual(MembershipStatus.USER_LOCKED, membershipService.Login("walter.poch@gmail.com", "345", out usuario));


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.IsTrue(usuario2.Membership.IsLockedOut);
            Assert.IsFalse(string.IsNullOrWhiteSpace(usuario2.Membership.LockedOutReason));

            Assert.AreEqual(MembershipStatus.USER_LOCKED, membershipService.Login("walter.poch@gmail.com", "testtest", out usuario));
        }

        [Test]
        public void PuedoPedirUnPasswordReset()
        {
            var token = string.Empty;
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");


            membershipService.RecoverPassword("walter.poch@gmail.com");
            token = usuario.Membership.PasswordResetToken;
            mailService.Verify(x => x.SendPasswordResetEmail(usuario, token));


            var usuario2 =
                Session.QueryOver<Usuario>().Where(u => u.Membership.Email == "walter.poch@gmail.com").SingleOrDefault();
            Assert.AreEqual(token, usuario2.Membership.PasswordResetToken);
        }

        [Test]
        public void PuedoCambiarElPasswordYLoguearmeConElNuevo()
        {
            var token = string.Empty;
            var cryptoService = new RijndaelEngine("WAL");
            var mailService = new Mock<IMailSenderService>();
            var formsService = new Mock<IFormAuthenticationStoreService>();

            var membershipService = new MembershipService(SessionFactory, cryptoService,
                                                          mailService.Object,
                                                          formsService.Object);

            var usuario = new Usuario();
            usuario.Nombre = "Walter";

            membershipService.CreateUser(usuario, "walter.poch@gmail.com", "testtest");

            membershipService.RecoverPassword("walter.poch@gmail.com");

            token = usuario.Membership.PasswordResetToken;
            mailService.Verify(x => x.SendPasswordResetEmail(usuario, token));

            membershipService.ChangePassword("walter.poch@gmail.com", token, "walter2");

            Assert.AreEqual(MembershipStatus.BAD_PASSWORD, membershipService.Login("walter.poch@gmail.com", "testtest", out usuario));

            Assert.AreEqual(MembershipStatus.USER_FOUND, membershipService.Login("walter.poch@gmail.com", "walter2", out usuario));
        }
    }
}