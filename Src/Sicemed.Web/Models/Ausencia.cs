using System;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Ausencia : Entity
    {
        public virtual Profesional Profesional { get; set; }
        public virtual DateTime Fecha { get; set; }
        public virtual TimeSpan? Desde { get; set; }
        public virtual TimeSpan? Hasta { get; set; }

        public Ausencia() { }

        public Ausencia(DateTime fecha, TimeSpan desde, TimeSpan hasta)
            : this(fecha)
        {
            Desde = desde;
            Hasta = hasta;
        }

        public Ausencia(DateTime fecha)
        {
            Fecha = fecha;
        }

        public virtual bool EnPeriodoDeAusencia(DateTime fecha)
        {            
            if(!Desde.HasValue && !Hasta.HasValue)
            {
                //Solo aplica la fecha
                return Fecha.ToMidnigth() == fecha.ToMidnigth();
            }
            else
            {
                //Si la fecha que me pasa esta incluida en la ausencia
                return fecha >= Fecha.ToMidnigth().Add(Desde.Value) 
                    && fecha <= Fecha.ToMidnigth().Add(Hasta.Value);
            }
        }
    }
}