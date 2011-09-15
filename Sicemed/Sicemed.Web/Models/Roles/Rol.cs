using System;

namespace Sicemed.Web.Models.Roles
{
    public abstract class Rol : Entity
    {
        public virtual DateTime FechaAsignacion { get; set; }

        protected Rol()
        {
            FechaAsignacion = DateTime.Now;
        }

        public abstract string DisplayName { get; }
    }
}