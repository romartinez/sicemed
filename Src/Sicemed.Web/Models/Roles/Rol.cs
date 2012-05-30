using System;

namespace Sicemed.Web.Models.Roles
{
    public abstract class Rol : Entity
    {
        public const string SECRETARIA = "Secretaria";
        public const string PACIENTE = "Paciente";
        public const string PROFESIONAL = "Profesional";
        public const string ADMINISTRADOR = "Administrador";

        protected Rol()
        {
            FechaAsignacion = DateTime.Now;
        }

        public virtual DateTime FechaAsignacion { get; set; }

        public virtual Persona Persona { get; set; }

        public abstract string DisplayName { get; }
    }
}