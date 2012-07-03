using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Turno : Entity
    {
        public enum EstadoTurno
        {
            Otorgado,
            Presentado,
            Atendido,
            Cancelado,
            Ausente
        }

        public enum EventoTurno
        {
            Obtener,
            Presentar,
            Atender,
            Cancelar,
            Ausentarse
        }

        public class CambioEstadoTurno
        {
            readonly EstadoTurno _currentState;
            readonly EventoTurno _eventoTurno;

            public CambioEstadoTurno(EstadoTurno currentState, EventoTurno eventoTurno)
            {
                _currentState = currentState;
                _eventoTurno = eventoTurno;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * _currentState.GetHashCode() + 31 * _eventoTurno.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                var other = obj as CambioEstadoTurno;
                return other != null && this._currentState == other._currentState && this._eventoTurno == other._eventoTurno;
            }
        }

        private static Dictionary<CambioEstadoTurno, EstadoTurno> _transiciones;

        #region Primitive Properties

        public virtual DateTime FechaGeneracion { get; protected set; }

        public virtual DateTime FechaTurno { get; protected set; }

        public virtual DateTime? FechaIngreso { get; protected set; }

        public virtual DateTime? FechaCancelacion { get; protected set; }

        public virtual DateTime? FechaAtencion { get; protected set; }

        public virtual string Nota { get; protected set; }

        public virtual string MotivoCancelacion { get; protected set; }

        public virtual string IpPaciente { get; protected set; }

        public virtual bool EsTelefonico { get; protected set; }

        public virtual EstadoTurno Estado { get; protected set; }

        public virtual DateTime FechaEstado { get; protected set; }

        #endregion

        #region Navigation Properties

        public virtual Paciente Paciente { get; protected set; }

        public virtual Profesional Profesional { get; protected set; }

        public virtual Secretaria SecretariaReservadoraTurno { get; protected set; }

        public virtual Secretaria SecretariaRecepcionista { get; set; }

        public virtual Persona CanceladoPor { get; set; }

        public virtual Especialidad Especialidad { get; protected set; }

        public virtual Consultorio Consultorio { get; set; }

        #endregion

        public virtual bool EsObtenidoWeb
        {
            get
            {
                return SecretariaReservadoraTurno == null;
            }
        }

        public virtual bool EsObtenidoPersonalmente
        {
            get
            {
                return SecretariaReservadoraTurno != null && !EsTelefonico;
            }
        }

        public virtual bool EsObtenidoTelefonicamente
        {
            get
            {
                return SecretariaReservadoraTurno != null && EsTelefonico;
            }
        }

        static Turno()
        {
            _transiciones = new Dictionary<CambioEstadoTurno, EstadoTurno>
            {
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Presentar), EstadoTurno.Presentado },
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Cancelar), EstadoTurno.Cancelado },
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Ausentarse), EstadoTurno.Ausente },
                { new CambioEstadoTurno(EstadoTurno.Presentado, EventoTurno.Atender), EstadoTurno.Atendido },
                { new CambioEstadoTurno(EstadoTurno.Presentado, EventoTurno.Cancelar), EstadoTurno.Cancelado },
            };
        }

        protected Turno()
        {
            Estado = EstadoTurno.Otorgado;
            FechaEstado = DateTime.Now;
        }

        public static EstadoTurno[] EstadosAplicaEvento(EventoTurno eventoTurno)
        {
            return Enum.GetValues(typeof(EstadoTurno))
                .Cast<EstadoTurno>()
                .Where(estado => PuedeAplicar(estado, eventoTurno)).ToArray();
        }

        public static bool PuedeAplicar(EstadoTurno estado, EventoTurno eventoTurno)
        {
            var transicion = new CambioEstadoTurno(estado, eventoTurno);
            return _transiciones.ContainsKey(transicion);
        }

        public virtual bool PuedeAplicar(EventoTurno eventoTurno)
        {
            var transicion = new CambioEstadoTurno(Estado, eventoTurno);
            return _transiciones.ContainsKey(transicion);
        }

        protected virtual EstadoTurno ProximoEstado(EventoTurno eventoTurno)
        {
            var transicion = new CambioEstadoTurno(Estado, eventoTurno);
            EstadoTurno proximoEstado;
            if (!_transiciones.TryGetValue(transicion, out proximoEstado))
                throw new Exception("Cambio de Estado inválido: " + Estado + " -> " + eventoTurno);
            return proximoEstado;
        }

        protected virtual EstadoTurno MoverEstado(EventoTurno eventoTurno)
        {
            Estado = ProximoEstado(eventoTurno);
            FechaEstado = DateTime.Now;
            return Estado;
        }

        public virtual Turno RegistrarIngreso(Secretaria secretariaRecepcionista)
        {
            MoverEstado(EventoTurno.Presentar);
            FechaIngreso = DateTime.Now;
            SecretariaRecepcionista = secretariaRecepcionista;
            //Reseteo de las inasistencias
            Paciente.ResetInasistencias();
            return this;
        }

        public virtual Turno RegistrarAtencion(string nota = null)
        {
            MoverEstado(EventoTurno.Atender);
            FechaAtencion = DateTime.Now;
            Nota = nota;
            return this;
        }

        public virtual Turno CancelarTurno(Persona canceladoPor, string motivoCancelacion)
        {
            MoverEstado(EventoTurno.Cancelar);
            FechaCancelacion = DateTime.Now;
            CanceladoPor = canceladoPor;
            MotivoCancelacion = motivoCancelacion;
            if (Paciente.Persona == canceladoPor) Paciente.AgregarInasistencia();
            return this;
        }

        public virtual Turno MarcarAusente()
        {
            MoverEstado(EventoTurno.Ausentarse);
            Paciente.AgregarInasistencia();
            return this;
        }

        #region Creates
        /// <summary>
        /// Crea un turno que es obtenido directamente por el cliente desde el browser.
        /// </summary>
        /// <returns></returns>
        public static Turno Create(
            DateTime fechaTurno,
            Paciente paciente,
            Profesional profesional,
            Especialidad especialidad,
            Consultorio consultorio,
            string ipPaciente)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            return new Turno
            {
                FechaGeneracion = DateTime.Now,
                FechaTurno = fechaTurno,
                Paciente = paciente,
                Profesional = profesional,
                Especialidad = especialidad,
                IpPaciente = ipPaciente,
                Consultorio = consultorio
            };
        }

        /// <summary>
        /// Crea un turno que es obtenido personalmente, o vía telefónica.
        /// </summary>
        public static Turno Create(
            DateTime fechaTurno,
            Paciente paciente,
            Profesional profesional,
            Especialidad especialidad,
            Secretaria secretariaReservadoraTurno,
            Consultorio consultorio,
            bool esTelefonico = false)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");
            if (consultorio == null) throw new ArgumentNullException("consultorio");
            if (secretariaReservadoraTurno == null) throw new ArgumentNullException("secretariaReservadoraTurno");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            return new Turno
            {
                FechaGeneracion = DateTime.Now,
                FechaTurno = fechaTurno,
                Paciente = paciente,
                Profesional = profesional,
                Especialidad = especialidad,
                SecretariaReservadoraTurno = secretariaReservadoraTurno,
                Consultorio = consultorio,
                EsTelefonico = esTelefonico,
            };
        }
        #endregion
    }
}