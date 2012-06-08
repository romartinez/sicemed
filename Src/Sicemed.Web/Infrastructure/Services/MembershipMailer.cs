using Mvc.Mailer;
using System.Net.Mail;
using SICEMED.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{ 
    public class MembershipMailer : MailerBase, IMembershipMailer     
	{
		public MembershipMailer():
			base()
		{
			MasterName="_Layout";
		}

        public MailMessage PasswordResetEmail(Persona user, string token)
        {
            if (MvcApplication.Clinica == null) return null; //Inicializacion
            var mailMessage = new MailMessage
            {
                Subject = "SICEMED - Recuperar Password"
            };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            ViewBag.Token = token;
            PopulateBody(mailMessage, viewName: "PasswordResetEmail");

            return mailMessage;
        }

        public MailMessage RegistrationEmail(Persona user)
        {
            if (MvcApplication.Clinica == null) return null; //Inicializacion
            var mailMessage = new MailMessage { Subject = "SICEMED - Bienvenido" };

            mailMessage.To.Add(user.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = user;
            PopulateBody(mailMessage, viewName: "RegistrationEmail");

            return mailMessage;
        }
	}
}