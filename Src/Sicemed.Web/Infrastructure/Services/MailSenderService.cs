using System.Net.Mail;
using System.Web.Mvc;
using Mvc.Mailer;
using SICEMED.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IMailSenderService
    {
        void SendPasswordResetEmail(Persona user, string token);
        void SendNewUserEmail(Persona user);
    }

    public class MailSenderService : MailerBase, IMailSenderService
    {

        public MailSenderService()
            : base()
        {
            MasterName = "_Layout";
        }

        public void SendPasswordResetEmail(Persona user, string token)
        {
            var mailMessage = new MailMessage
                                  {
                                      Subject = "SICEMED - Recuperar Password"
                                  };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            ViewBag.Token = token;
            PopulateBody(mailMessage, "RecuperarPassword");

            mailMessage.Send();
        }

        public void SendNewUserEmail(Persona user)
        {
            var mailMessage = new MailMessage
            {
                Subject = "SICEMED - Bienvenido"
            };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            PopulateBody(mailMessage, "Registro");

            mailMessage.Send();
        }
    }
}