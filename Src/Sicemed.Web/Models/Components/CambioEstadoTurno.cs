using System;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.Components
{
    public class CambioEstadoTurno : ComponentBase
    {
        public virtual EstadoTurno Estado { get; set; }
        public virtual EventoTurno Evento { get; set; }

        public virtual Persona Responsable { get; set; }
        public virtual DateTime Fecha { get; set; }

        protected CambioEstadoTurno(){}

        public CambioEstadoTurno(EstadoTurno currentState, EventoTurno eventoTurno)
        {
            Estado = currentState;
            Evento = eventoTurno;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * Estado.GetHashCode() + 31 * Evento.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as CambioEstadoTurno;
            return other != null && this.Estado == other.Estado && this.Evento == other.Evento;
        } 
    }
}