using System;

namespace Sicemed.Web.Models.Roles
{
    public class Secretaria : Rol
    {
        public override string DisplayName
        {
            get { return SECRETARIA; }
        }

        public virtual DateTime FechaIngreso { get; set; }

        protected Secretaria() {}

        public static Rol Create(DateTime fechaIngreso)
        {
            return new Secretaria() { FechaIngreso = fechaIngreso };
        }
    }
}