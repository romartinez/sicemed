using System.Security.Principal;

namespace Sicemed.Model {
    public class PrincipalBase : GenericPrincipal {
        public PrincipalBase(IdentityBase identity, string[] roles) : base(identity, roles) {}

        public virtual IdentityBase IdentityBase {
            get {
                return (IdentityBase)Identity;
            }
        }

    }
}
