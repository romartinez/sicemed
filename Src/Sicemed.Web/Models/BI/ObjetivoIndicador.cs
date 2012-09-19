using System;
using Sicemed.Web.Models.BI.Enumerations;

namespace Sicemed.Web.Models.BI
{
    public class ObjetivoIndicador : Entity
    {
        public virtual Indicador Indicador { get; set; }

        public virtual decimal? Valor { get; set; }
        public virtual DateTime? FechaLectura { get; set; }

        public virtual int Anio { get; set; }
        public virtual int Mes { get; set; }
      
        public virtual decimal ValorMinimo { get; set; }
        public virtual decimal ValorMaximo { get; set; }

        public virtual EstadoObjetivo Estado
        {
            get
            {
                if (!Valor.HasValue)
                    return EstadoObjetivo.SinDatos;
                if (Valor >= ValorMinimo && Valor <= ValorMaximo)
                    return EstadoObjetivo.Verde;
                return EstadoObjetivo.Rojo;
            }
        }

        public virtual void AsignarValor(decimal valor)
        {
            Valor = valor;
            FechaLectura = DateTime.Now;
        }
    }
}