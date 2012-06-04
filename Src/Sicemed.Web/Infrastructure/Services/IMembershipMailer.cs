using System.Net.Mail;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IMembershipMailer
    {
        MailMessage PasswordResetEmail(Persona user, string token);
        MailMessage RegistrationEmail(Persona user);
    }
}