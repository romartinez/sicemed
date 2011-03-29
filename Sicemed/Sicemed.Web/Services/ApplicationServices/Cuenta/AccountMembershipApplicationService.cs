﻿using System;
using System.Web;
using System.Web.Security;
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

        public int MinPasswordLength
        {
            get { return _provider.MinRequiredPasswordLength; }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Value cannot be null or empty.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password))
                throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword))
                throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword))
                throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying CambiarPassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            } catch (ArgumentException)
            {
                return false;
            } catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public PrincipalBase GetCurrentUser()
        {
            if(!HttpContext.Current.User.Identity.IsAuthenticated) return null;
            return (PrincipalBase)HttpContext.Current.User;
        }

        #endregion
    }
}