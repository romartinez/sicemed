using System;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Turno : Entity
    {
        #region Primitive Properties

        public virtual DateTime FechaGeneracion { get; protected set; }

        public virtual DateTime FechaTurno { get; protected set; }

        public virtual DateTime? FechaIngreso { get; protected set; }

        public virtual DateTime? FechaAtencion { get; protected set; }

        public virtual string Nota { get; protected set; }

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

        public virtual bool SePresento
        {
            get { return FechaIngreso.HasValue; }
        }

        public virtual bool SeAtendio
        {
            get { return FechaAtencion.HasValue; }
        }

        protected Turno() { }

        public virtual void RegistrarIngreso(Secretaria secretariaRecepcionista)
        {
            if (FechaIngreso.HasValue) throw new NotSupportedException("No se puede marcar el Turno como Ingreso, ya lo estaba.");
            FechaIngreso = DateTime.Now;
            SecretariaRecepcionista = secretariaRecepcionista;
            //Reseteo de las inasistencias
            Paciente.ResetInasistencias();            
        }

        public virtual void RegistrarAtencion(string nota = null)
        {
            if (!FechaIngreso.HasValue) throw new NotSupportedException("No se puede marcar el Turno como Atendido, nunca se registró su Ingreso.");
            if (FechaAtencion.HasValue) throw new NotSupportedException("No se puede marcar el Turno como Atendido, ya lo estaba.");
            FechaAtencion = DateTime.Now;
            Nota = nota;
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