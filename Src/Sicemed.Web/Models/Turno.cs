using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Turno : Entity
    {

        private static readonly Dictionary<CambioEstadoTurno, EstadoTurno> Transiciones;

        #region Primitive Properties

        public virtual DateTime FechaTurno { get; protected set; }

        public virtual TimeSpan DuracionTurno { get; protected set; }

        public virtual string Nota { get; protected set; }

        public virtual string MotivoCancelacion { get; protected set; }

        public virtual string IpPaciente { get; protected set; }

        public virtual bool EsTelefonico { get; protected set; }

        public virtual bool EsSobreTurno { get; protected set; }
        
        public virtual bool EsObtenidoWeb { get; protected set; }

        public virtual EstadoTurno Estado { get; protected set; }

        public virtual DateTime FechaEstado { get; protected set; }
        public virtual string NumeroAfiliado { get; set; }
        public virtual Plan Plan { get; set; }
        public virtual decimal Coseguro { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Paciente Paciente { get; protected set; }

        public virtual Profesional Profesional { get; protected set; }

        public virtual Especialidad Especialidad { get; protected set; }

        public virtual Consultorio Consultorio { get; set; }

        public virtual IList<CambioEstadoTurno> CambiosDeEstado { get; set; }

        #endregion

        #region Propiedades Calculadas

        public virtual DateTime FechaTurnoFinal
        {
            get { return FechaTurno.Add(DuracionTurno); }
        }

        public virtual DateTime FechaGeneracion
        {
            get { return ObtenerFechaEstado(EventoTurno.Obtener).Value; }
        }

        public virtual DateTime? FechaIngreso
        {
            get { return ObtenerFechaEstado(EventoTurno.Presentar); }
        }

        public virtual DateTime? FechaCancelacion
        {
            get { return ObtenerFechaEstado(EventoTurno.Cancelar); }
        }

        public virtual DateTime? FechaAtencion
        {
            get { return ObtenerFechaEstado(EventoTurno.Atender); }
        }

        public virtual Secretaria SecretariaReservadoraTurno
        {
            get
            {
                var persona = ObtenerResponsableEstado(EventoTurno.Obtener);
                if (!persona.IsInRole<Secretaria>()) return null;
                return persona.As<Secretaria>();
            }
        }

        public virtual Secretaria SecretariaRecepcionista
        {
            get
            {
                var persona = ObtenerResponsableEstado(EventoTurno.Obtener);
                if (persona == null) return null;
                return persona.As<Secretaria>();
            }
        }

        public virtual Persona CanceladoPor
        {
            get
            {
                return ObtenerResponsableEstado(EventoTurno.Cancelar);
            }
        }

        public virtual bool EsObtenidoPersonalmente
        {
            get
            {
                return !EsObtenidoWeb && !EsTelefonico;
            }
        }

        public virtual bool EsObtenidoTelefonicamente
        {
            get
            {
                return !EsObtenidoWeb && EsTelefonico;
            }
        }

        #endregion

      
        static Turno()
        {
            Transiciones = new Dictionary<CambioEstadoTurno, EstadoTurno>
            {
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Presentar), EstadoTurno.Presentado },
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Cancelar), EstadoTurno.Cancelado },
                { new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Ausentarse), EstadoTurno.Ausente },
                { new CambioEstadoTurno(EstadoTurno.Presentado, EventoTurno.Atender), EstadoTurno.Atendido },
                { new CambioEstadoTurno(EstadoTurno.Presentado, EventoTurno.Cancelar), EstadoTurno.Cancelado },
            };
        }

        protected Turno() { }

        protected Turno(Persona persona)
        {
            var fecha = DateTime.Now;
            CambiosDeEstado = new List<CambioEstadoTurno>
                {
                    new CambioEstadoTurno(EstadoTurno.Otorgado, EventoTurno.Obtener)
                        {
                            Fecha = fecha, Responsable = persona
                        }
                };
            Estado = EstadoTurno.Otorgado;
            FechaEstado = fecha;
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
            return Transiciones.ContainsKey(transicion);
        }

        public virtual bool PuedeAplicar(EventoTurno eventoTurno)
        {
            var transicion = new CambioEstadoTurno(Estado, eventoTurno);
            return Transiciones.ContainsKey(transicion);
        }

        protected virtual EstadoTurno ProximoEstado(EventoTurno eventoTurno)
        {
            var transicion = new CambioEstadoTurno(Estado, eventoTurno);
            EstadoTurno proximoEstado;
            if (!Transiciones.TryGetValue(transicion, out proximoEstado))
                throw new Exception("Cambio de Estado inválido: " + Estado + " -> " + eventoTurno);
            return proximoEstado;
        }

        protected virtual EstadoTurno MoverEstado(EventoTurno eventoTurno, Persona persona)
        {
            var estado = ProximoEstado(eventoTurno);
            var fecha = DateTime.Now;
            var cambioEstado = new CambioEstadoTurno(estado, eventoTurno) { Fecha = fecha, Responsable = persona };
            CambiosDeEstado.Add(cambioEstado);
            Estado = estado;
            FechaEstado = fecha;
            return estado;
        }

        public virtual Turno RegistrarIngreso(Secretaria secretariaRecepcionista)
        {
            MoverEstado(EventoTurno.Presentar, secretariaRecepcionista.Persona);
            //Reseteo de las inasistencias
            Paciente.ResetInasistencias();
            return this;
        }

        public virtual Turno RegistrarAtencion(Profesional profesional, string nota = null)
        {
            MoverEstado(EventoTurno.Atender, profesional.Persona);
            Nota = nota;
            return this;
        }

        public virtual Turno CancelarTurno(Persona canceladoPor, string motivoCancelacion)
        {
            MoverEstado(EventoTurno.Cancelar, canceladoPor);
            MotivoCancelacion = motivoCancelacion;
            if (Paciente.Persona == canceladoPor) Paciente.AgregarInasistencia();
            return this;
        }

        public virtual Turno MarcarAusente()
        {
            MoverEstado(EventoTurno.Ausentarse, null);
            Paciente.AgregarInasistencia();
            return this;
        }

        public virtual Turno RegistrarNota(Profesional profesional, string nota = null)
        {            
            Nota += Environment.NewLine 
                + new string('-', 32)
                + Environment.NewLine + nota;
            return this;
        }

        private CambioEstadoTurno ObtenerCambioEstado(EventoTurno evento)
        {
            return CambiosDeEstado.SingleOrDefault(x => x.Evento == evento);
        }

        private DateTime? ObtenerFechaEstado(EventoTurno evento)
        {
            var cambioEstado = ObtenerCambioEstado(evento);
            if (cambioEstado == null) return null;
            return cambioEstado.Fecha;
        }

        private Persona ObtenerResponsableEstado(EventoTurno evento)
        {
            var cambioEstado = ObtenerCambioEstado(evento);
            if (cambioEstado == null) return null;
            return cambioEstado.Responsable;
        }


        #region Creates
        /// <summary>
        /// Crea un turno que es obtenido directamente por el cliente desde el browser.
        /// </summary>
        /// <returns></returns>
        public static Turno Create(
            DateTime fechaTurno,
            TimeSpan duracionTurno,
            Paciente paciente,
            Profesional profesional,
            Especialidad especialidad,
            Consultorio consultorio,
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
            Plan plan,
            String numeroAfiliado,
            Decimal coseguro,
            string ipPaciente)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            var turno = new Turno(paciente.Persona)
            {
                FechaTurno = fechaTurno,
                Paciente = paciente,
                Profesional = profesional,
                Especialidad = especialidad,
                IpPaciente = ipPaciente,
                Consultorio = consultorio,
                DuracionTurno=duracionTurno,
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                Plan=plan,
                NumeroAfiliado=numeroAfiliado,
                Coseguro=coseguro,
                EsObtenidoWeb = true
            };

            return turno;
        }

        /// <summary>
        /// Crea un turno que es obtenido personalmente, o vía telefónica.
        /// </summary>
        public static Turno Create(
            DateTime fechaTurno,
            TimeSpan duracionTurno,
            Paciente paciente,
            Profesional profesional,
            Especialidad especialidad,
            Secretaria secretariaReservadoraTurno,
            Consultorio consultorio,
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO            
            Plan plan,
            String numeroAfiliado,
            Decimal coseguro,
            bool esTelefonico = false
            )
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");
            if (consultorio == null) throw new ArgumentNullException("consultorio");
            if (secretariaReservadoraTurno == null) throw new ArgumentNullException("secretariaReservadoraTurno");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            var turno = new Turno(secretariaReservadoraTurno.Persona)
            {
                FechaTurno = fechaTurno,
                DuracionTurno = duracionTurno,
                Paciente = paciente,
                Profesional = profesional,
                Especialidad = especialidad,
                Consultorio = consultorio,
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                Plan = plan,
                NumeroAfiliado = numeroAfiliado,
                Coseguro = coseguro,
                EsTelefonico = esTelefonico,
                EsSobreTurno = false,
                EsObtenidoWeb = false
            };

            return turno;
        }
        #endregion

        public static Turno CreateSobreTurno(DateTime fechaTurno, TimeSpan duracionTurno, Paciente paciente, Profesional profesional, Especialidad especialidad, Secretaria secretariaReservadoraTurno, Plan plan,String numeroAfiliado,Decimal coseguro, bool esTelefonico)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");
            if (secretariaReservadoraTurno == null) throw new ArgumentNullException("secretariaReservadoraTurno");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            var turno = new Turno(secretariaReservadoraTurno.Persona)
            {
                FechaTurno = fechaTurno,
                DuracionTurno = duracionTurno,
                Paciente = paciente,
                Profesional = profesional,
                Especialidad = especialidad,
//RM SE AGREGA DATOS DE LA FORMA DE PAGO AL TURNO
                Plan = plan,
                NumeroAfiliado = numeroAfiliado,
                Coseguro = coseguro,
                EsTelefonico = esTelefonico,
                EsSobreTurno = true,
                EsObtenidoWeb = false
            };

            return turno;
        }
    }
}