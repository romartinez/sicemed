using System;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.Roles
{
    public class Secretaria : Rol
    {
        public override string DisplayName
        {
            get { return "Secretaria"; }
        }

        public virtual DateTime FechaIngreso { get; set; }               

        protected Secretaria(){}
        public Secretaria(DateTime fechaIngreso)
        {
            FechaIngreso = fechaIngreso;
        }
    }
}