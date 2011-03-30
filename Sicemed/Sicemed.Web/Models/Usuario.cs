using System;
using System.Linq;
using System.Security.Principal;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Components.Documentos;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Usuario : Entity, IIdentity, IPrincipal
    {
        #region Primitive Properties

        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual Documento Documento { get; set; }
        
        public virtual Domicilio Domicilio { get; set; }

        public virtual bool? EstaHabilitadoTurnosWeb { get; set; }

        public virtual Telefono Telefono { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual string NumeroAfiliado { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Turno> Turnos { get; set; }

        public virtual ISet<Rol> Roles { get; set; }

        #endregion

        public string Name { get { return NombreUsuario; } }
        public string NombreUsuario { get; set; }

        public string Nombre { get; set; }
        public string SegundoNombre { get; set; }
        public string Apellido{ get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string AuthenticationType
        {
            get { return "SICEMED"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(Name); }
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(rol => string.Equals(role, rol.Nombre, StringComparison.CurrentCultureIgnoreCase));
        }

        public IIdentity Identity
        {
            get { return this; }
        }
    }
}