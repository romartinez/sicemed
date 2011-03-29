using System.Web.Security;
using Sicemed.Web.Plumbing;

namespace Sicemed.Web.Services.ApplicationServices.Cuenta
{
    public interface IMembershipApplicationService
    {
        int MinPasswordLength { get; }
        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        PrincipalBase GetCurrentUser();
    }
}