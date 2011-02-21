using System.Security.Principal;

namespace Sicemed.Model {
    public class PrincipalBase : GenericPrincipal {
        public PrincipalBase(IdentityBase identity, string[] roles) : base(identity, roles) {
        }

        public virtual IdentityBase IdentityBase {
            get { return (IdentityBase) Identity; }
        }

        //public bool IsSecretaria { 
        //    get {
        //        return this.IsInRole(Roles.SECRETARIA.ToString());
        //    }
        //}

        //public bool IsAdmin {
        //    get {
        //        return this.IsInRole(Roles.ADMIN.ToString());
        //    }
        //}

        //public bool IsPaciente {
        //    get {
        //        return this.IsInRole(Roles.PACIENTE.ToString());
        //    }
        //}

        //public bool IsProfesional {
        //    get {
        //        return this.IsInRole(Roles.PROFESIONAL.ToString());
        //    }        
        //}
    }
}