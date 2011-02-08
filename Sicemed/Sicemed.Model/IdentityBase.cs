using System.Collections.Generic;
using System.Security.Principal;

namespace Sicemed.Model {
    public class IdentityBase : GenericIdentity {
        public IdentityBase(string name) : base(name) {
        }

        public IdentityBase(string name, string authenticationType) : base(name, authenticationType) {
        }

        public Dictionary<string, object> ExtendedData { get; set; }
    }
}