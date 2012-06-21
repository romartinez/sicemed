using System.Net.Mail;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface INotificationService
    {
        MailMessage CancelacionTurno(Persona user, Turno turno);
    }
}