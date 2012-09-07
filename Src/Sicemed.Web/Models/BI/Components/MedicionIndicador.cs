using System;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models.BI.Components
{
    public class MedicionIndicador : ComponentBase
    {
        public virtual decimal Valor { get; set; }
        public virtual DateTime FechaLectura { get; set; }

        public MedicionIndicador(decimal valor) : this(valor, DateTime.Now) { }

        public MedicionIndicador(decimal valor, DateTime fecha)
        {
            Valor = valor;
            FechaLectura = fecha;
        }
    }
}