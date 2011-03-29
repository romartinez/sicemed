using System;
using System.Web;
using System.Web.Security;
using Sicemed.Web.Models;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public class AccountMembershipApplicationService : IMembershipApplicationService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipApplicationService()
            : this(null) {}

        public AccountMembershipApplicationService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        #region IMembershipApplicationService Members

        public int LargoMinimoPassword
        {
            get { return _provider.MinRequiredPasswordLength; }
        }

        public bool ValidarUsuario(string nombreUsuario, string password)
        {
            if (String.IsNullOrEmpty(nombreUsuario))
                throw new ArgumentException("Value cannot be null or empty.", "nombreUsuario");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(nombreUsuario, password);
        }

        public MembershipCreateStatus CrearUsuario(string nombreUsuario, string password, string email)
        {
            if (String.IsNullOrEmpty(nombreUsuario))
                throw new ArgumentException("Value cannot be null or empty.", "nombreUsuario");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(nombreUsuario, password, email, null, null, true, null, out status);
            return status;
        }

        public bool CambiarPassword(string nombreUsuario, string passwordViejo, string passwordNuevo)
        {
            if (String.IsNullOrEmpty(nombreUsuario))
                throw new ArgumentException("Value cannot be null or empty.", "nombreUsuario");
            if (String.IsNullOrEmpty(passwordViejo))
                throw new ArgumentException("Value cannot be null or empty.", "passwordViejo");
            if (String.IsNullOrEmpty(passwordNuevo))
                throw new ArgumentException("Value cannot be null or empty.", "passwordNuevo");

            // The underlying CambiarPassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(nombreUsuario, true /* userIsOnline */);
                return currentUser.ChangePassword(passwordViejo, passwordNuevo);
            } catch (ArgumentException)
            {
                return false;
            } catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public Usuario GetCurrentUser()
        {
            if(!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (Usuario)HttpContext.Current.User;
        }

        #endregion
    }
}