using System.Net.Mail;
using Mvc.Mailer;
using SICEMED.Web;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public class NotificationService : MailerBase, INotificationService
    {
        public MailMessage CancelacionTurno(Persona user, Turno turno)
        {            
            var mailMessage = new MailMessage
            {
                Subject = string.Format("SICEMED - Cancelación Turno: {0} {1}", 
                turno.FechaTurno.ToShortDateString(), turno.FechaTurno.ToShortTimeString())
            };

            mailMessage.Bcc.Add(turno.Profesional.Persona.Membership.Email);
            mailMessage.Bcc.Add(turno.Paciente.Persona.Membership.Email);
            mailMessage.From = new MailAddress(MvcApplication.Clinica.Email);
            ViewData.Model = turno;
            ViewBag.Usuario = user.NombreCompleto;
            PopulateBody(mailMessage, viewName: "CancelacionTurno");

            return mailMessage;
        }
    }
}