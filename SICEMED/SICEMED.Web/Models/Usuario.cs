using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Components.Documentos;

namespace Sicemed.Web.Models
{
    public class Usuario : Entity, IPrincipal
    {
        private readonly Membership _membership;
        private readonly IList<Rol> _roles;

        public Usuario()
        {
            _roles = new List<Rol>();           
            _membership = new Membership();
        }

        public virtual Membership Membership
        {
            get { return _membership; }
        }

        public virtual IEnumerable<Rol> Roles
        {
            get { return _roles; }
        }

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

        #endregion

        public virtual string Nombre { get; set; }
        public virtual string SegundoNombre { get; set; }
        public virtual string Apellido{ get; set; }
        
        public virtual bool IsInRole(string role)
        {
            return _roles.Select(r => r.Name).Contains(role);
        }

        public virtual IIdentity Identity
        {
            get { return new GenericIdentity(this.Membership.Email); }
        }

        public virtual Usuario AgregarRol(Rol rol)
        {
            _roles.Add(rol);
            return this;
        }

        public virtual Usuario QuitarRol(Rol rol)
        {
            _roles.Remove(rol);
            return this;
        }

        public override string ToString()
        {
            return Membership.Email;
        }
    }
}