using System;

namespace Sicemed.Web.Models.Roles
{
    public abstract class Rol : Entity
    {
        public const string SECRETARIA = "Secretaria";
        public const string PACIENTE = "Paciente";
        public const string PROFESIONAL = "Profesional";
        public const string ADMINISTRADOR = "Administrador";

        public virtual DateTime FechaAsignacion { get; set; }

        protected Rol()
        {
            FechaAsignacion = DateTime.Now;
        }

        public abstract string DisplayName { get; }
    }
}