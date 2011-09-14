using System;

namespace Sicemed.Web.Models.Components.Roles
{
    public abstract class Rol : ComponentBase
    {
        public static readonly Rol Secretaria = new Secretaria();
        public static readonly Rol Profesional = new Profesional();
        public static readonly Rol Administrador = new Administrador();

        public static readonly Rol[] Roles = new[] { Secretaria, Profesional, Administrador };

        public virtual DateTime FechaAsignacion { get; set; }
    }
}