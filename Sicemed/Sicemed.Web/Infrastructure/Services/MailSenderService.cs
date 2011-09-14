using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IMailSenderService
    {
        void SendPasswordResetEmail(Persona user, string token);
        void SendNewUserEmail(Persona user);
    }

    public class MailSenderService : IMailSenderService
    {
        #region IMailSenderService Members

        public void SendPasswordResetEmail(Persona user, string token)
        {
            //TODO: throw new NotImplementedException();
        }

        public void SendNewUserEmail(Persona user)
        {
            //TODO: throw new NotImplementedException();
        }

        #endregion
    }
}