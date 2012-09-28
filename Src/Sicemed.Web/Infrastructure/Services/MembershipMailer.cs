using System.Net.Mail;
using Mvc.Mailer;
using SICEMED.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public class MembershipMailer : MailerBase, IMembershipMailer
    {
        public MembershipMailer() :
            base()
        {
            MasterName = "_MailLayout";
        }

        public MailMessage PasswordResetEmail(Persona user, string token)
        {
            if (MvcApplication.Clinica == null) return null; //Inicializacion
            var mailMessage = new MailMessage
            {
                Subject = string.Format("{0} - Recuperar Password", MvcApplication.Clinica.RazonSocial)
            };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            ViewBag.Token = token;
            PopulateBody(mailMessage, "PasswordResetEmail");

            return mailMessage;
        }

        public MailMessage RegistrationEmail(Persona user)
        {
            if (MvcApplication.Clinica == null) return null; //Inicializacion
            var mailMessage = new MailMessage
                {
                    Subject = string.Format("{0} - Bienvenido", MvcApplication.Clinica.RazonSocial)
                };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            PopulateBody(mailMessage, "RegistrationEmail");

            return mailMessage;
        }
    }
}