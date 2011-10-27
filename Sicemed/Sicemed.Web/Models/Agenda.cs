using System;
using Iesi.Collections.Generic;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Models
{
    public class Agenda : Entity
    {
        private readonly ISet<Especialidad> _especialidadesAtendidas;

        public Agenda()
        {
            _especialidadesAtendidas = new HashedSet<Especialidad>();
        }

        public virtual DateTime HorarioDesde { get; set; }
        public virtual DateTime HorarioHasta { get; set; }

        public virtual TimeSpan DuracionTurno { get; set; }

        public virtual DayOfWeek Dia { get; set; }

        public virtual Consultorio Consultorio { get; set; }
        public virtual Profesional Profesional { get; set; }

        public virtual ISet<Especialidad> EspecialidadesAtendidas
        {
            get { return _especialidadesAtendidas; }
        }

        public virtual void AgregarEspecialidad(Especialidad especialidad)
        {
            if (especialidad == null) throw new ArgumentNullException("especialidad");

            _especialidadesAtendidas.Add(especialidad);
        }
    }
}