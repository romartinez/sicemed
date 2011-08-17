using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Services
{
    public interface IMailSenderService
    {
        void SendPasswordResetEmail(Usuario user, string token);
        void SendNewUserEmail(Usuario user);
    }

    public class MailSenderService : IMailSenderService
    {
        #region IMailSenderService Members

        public void SendPasswordResetEmail(Usuario user, string token)
        {
            //TODO: throw new NotImplementedException();
        }

        public void SendNewUserEmail(Usuario user)
        {
            //TODO: throw new NotImplementedException();
        }

        #endregion
    }
}