using System;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Turno : Entity
    {
        #region Primitive Properties

        public virtual DateTime FechaGeneracion { get; protected set; }

        public virtual DateTime FechaTurno { get; protected set; }

        public virtual DateTime? FechaIngreso { get; set; }

        public virtual DateTime? FechaAtencion { get; set; }

        public virtual string Nota { get; set; }

        public virtual string IpPaciente { get; protected set; }
        
        public virtual bool EsTelefonico { get; protected set; }

        #endregion

        #region Navigation Properties

        public virtual Paciente Paciente { get; protected set; }

        public virtual Profesional Profesional { get; protected set; }

        public virtual Secretaria SecretariaReservadoraTurno { get; protected set; }
        
        public virtual Secretaria SecretariaRecepcionista { get; set; }

        public virtual Especialidad Especialidad { get; protected set; }

        public virtual Consultorio Consultorio { get; set; }

        public virtual Agenda Agenda { get; protected set; }

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

        protected Turno(){}

        /// <summary>
        /// Crea un turno que es obtenido directamente por el cliente desde el browser.
        /// </summary>
        /// <param name="fechaTurno"></param>
        /// <param name="paciente"></param>
        /// <param name="profesional"></param>
        /// <param name="especialidad"></param>
        /// <param name="ipPaciente"></param>        
        /// <param name="agenda"></param>
        /// <returns></returns>
        public static Turno Create(
            DateTime fechaTurno,
            Paciente paciente,
            Profesional profesional,
            Especialidad especialidad,
            string ipPaciente,            
            Agenda agenda = null)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");

            if (!profesional.Especialidades.Contains(especialidad)) 
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            if (agenda != null && !profesional.Agendas.Contains(agenda))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende en el momento elegido.", "agenda");

            if(agenda != null && !agenda.EspecialidadesAtendidas.Contains(especialidad))
                throw new ArgumentException(@"La Especialidad elegida no se encuentra disponible en la agenda del profesional.", "especialidad");

            return new Turno
                       {
                           FechaGeneracion = DateTime.Now,
                           FechaTurno = fechaTurno,
                           Paciente = paciente,
                           Profesional = profesional,
                           Especialidad = especialidad,
                           IpPaciente = ipPaciente,
                           Consultorio = agenda.Consultorio,
                           Agenda = agenda
                       };
        }

        /// <summary>
        /// Crea un turno que es obtenido personalmente, o vía telefónica.
        /// </summary>
        /// <param name="fechaTurno"></param>
        /// <param name="paciente"></param>
        /// <param name="profesional"></param>
        /// <param name="especialidad"></param>
        /// <param name="secretariaReservadoraTurno"></param>
        /// <param name="esTelefonico"></param>
        /// <param name="agenda"></param>
        /// <returns></returns>
        public static Turno Create(
            DateTime fechaTurno, 
            Paciente paciente, 
            Profesional profesional, 
            Especialidad especialidad,
            Secretaria secretariaReservadoraTurno,
            bool esTelefonico = false,
            Agenda agenda = null)
        {
            if (paciente == null) throw new ArgumentNullException("paciente");
            if (profesional == null) throw new ArgumentNullException("profesional");
            if (especialidad == null) throw new ArgumentNullException("especialidad");
            if (secretariaReservadoraTurno == null) throw new ArgumentNullException("secretariaReservadoraTurno");

            if (!profesional.Especialidades.Contains(especialidad))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende la Especialidad seleccionada", "especialidad");

            if (agenda != null && !profesional.Agendas.Contains(agenda))
                throw new ArgumentException(@"El Profesional seleccionado para el turno no atiende en el momento elegido.", "agenda");

            return new Turno
                       {
                           FechaGeneracion = DateTime.Now,
                           FechaTurno = fechaTurno,
                           Paciente = paciente,
                           Profesional = profesional,
                           Especialidad = especialidad,
                           SecretariaReservadoraTurno = secretariaReservadoraTurno,
                           EsTelefonico = esTelefonico,
                           Agenda = agenda
                       };
        }
    }
}