using System;
using System.Linq;
using System.Security.Principal;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Persona : Entity, IPrincipal
    {
        private readonly Membership _membership;
        private readonly ISet<Rol> _roles;

        public Persona()
        {
            _roles = new HashedSet<Rol>();
            _membership = new Membership();
        }

        public virtual Membership Membership
        {
            get { return _membership; }
        }

        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string SegundoNombre { get; set; }

        public virtual string Apellido { get; set; }

        public virtual string NombreCompleto
        {
            get { return string.Format("{0}, {1} {2}", Apellido, Nombre, SegundoNombre); }
        }

        public virtual DateTime? FechaNacimiento { get; set; }

        public virtual decimal? Peso { get; set; }

        public virtual decimal? Altura { get; set; }

        public virtual Documento Documento { get; set; }

        public virtual Domicilio Domicilio { get; set; }

        public virtual Telefono Telefono { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<Rol> Roles
        {
            get { return _roles; }
        }

        #endregion

        #region IPrincipal Members

        public virtual bool IsInRole(string role)
        {
            return _roles.Select(r => r.DisplayName).Contains(role);
        }

        public virtual IIdentity Identity
        {
            get { return new GenericIdentity(this.Membership.Email); }
        }

        #endregion

        public virtual int? Edad
        {
            get
            {
                if (!FechaNacimiento.HasValue) return null;
                return Convert.ToInt32(Math.Abs(DateTime.Now.Subtract(FechaNacimiento.Value).TotalDays / 365));
            }
        }

        public virtual bool IsInRole<T>() where T : Rol
        {
            return IsInRole(typeof(T));
        }

        public virtual bool IsInRole(Type rolType)
        {
            return _roles.Any(x => x.GetType() == rolType);
        }

        public virtual T As<T>() where T : Rol
        {
            var personaAs = _roles.FirstOrDefault(x => x is T) as T;
            if (personaAs == default(T))
                throw new IdentityNotMappedException("El usuario no es del tipo " + typeof(T).Name);
            return personaAs;
        }

        public virtual Persona AgregarRol(Rol rol)
        {
            _roles.Add(rol);
            rol.Persona = this;
            return this;
        }

        public virtual Persona QuitarRol(Rol rol)
        {
            var roleToRemove = _roles.FirstOrDefault(r => r == rol);
            if (roleToRemove != null) _roles.Remove(roleToRemove);
            return this;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", base.ToString(), Membership);
        }
    }
}