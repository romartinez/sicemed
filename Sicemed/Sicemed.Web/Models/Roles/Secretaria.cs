using System;

namespace Sicemed.Web.Models.Roles
{
    public class Secretaria : Rol
    {
        protected Secretaria() {}

        public override string DisplayName
        {
            get { return SECRETARIA; }
        }

        public virtual DateTime FechaIngreso { get; set; }

        public static Rol Create(DateTime fechaIngreso)
        {
            return new Secretaria {FechaIngreso = fechaIngreso};
        }
    }
}