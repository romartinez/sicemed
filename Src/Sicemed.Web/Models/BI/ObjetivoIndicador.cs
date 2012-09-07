using System;
using Sicemed.Web.Models.BI.Components;

namespace Sicemed.Web.Models.BI
{
    public class ObjetivoIndicador : Entity
    {
        public virtual Indicador Indicador { get; set; }

        public virtual MedicionIndicador Medicion { get; set; }

        public virtual int Anio { get; set; }
        public virtual int Mes { get; set; }
      
        public virtual decimal ValorMinimo { get; set; }
        public virtual decimal ValorMaximo { get; set; }
    }
}