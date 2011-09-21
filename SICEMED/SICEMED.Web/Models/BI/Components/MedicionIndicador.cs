using System;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.BI.Components
{
    public class MedicionIndicador : ComponentBase
    {
        public virtual double Valor { get; set; }
        public virtual DateTime Fecha { get; set; }
    }
}