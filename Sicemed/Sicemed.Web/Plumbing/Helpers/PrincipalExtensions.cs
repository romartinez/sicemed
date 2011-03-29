using System.Security.Principal;
using Sicemed.Web.Models;

// ReSharper disable CheckNamespace
namespace Sicemed.Web.Plumbing.Helpers {
// ReSharper restore CheckNamespace
    public static class PrincipalExtensions {
        public static PrincipalBase GetCustom(this IPrincipal principal) {
            return (PrincipalBase) principal;
        }
    }
}