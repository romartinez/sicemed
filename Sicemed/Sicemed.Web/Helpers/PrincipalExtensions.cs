using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Sicemed.Model;

// ReSharper disable CheckNamespace
namespace System.Web {
// ReSharper restore CheckNamespace
    public static class PrincipalExtensions {
        public static PrincipalBase GetCustom(this IPrincipal principal) {
            return (PrincipalBase) principal;
        }
    }
}