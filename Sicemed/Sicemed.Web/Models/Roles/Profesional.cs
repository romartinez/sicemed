using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        private readonly ISet<Agenda> _agendas;
        private readonly ISet<Especialidad> _especialidades;

        protected Profesional()
        {
            _especialidades = new HashSet<Especialidad>();
            _agendas = new HashSet<Agenda>();
        }

        public override string DisplayName
        {
            get { return PROFESIONAL; }
        }

        public virtual ISet<Especialidad> Especialidades
        {
            get { return _especialidades; }
        }

        public virtual ISet<Agenda> Agendas
        {
            get { return _agendas; }
        }

        public virtual string Matricula { get; set; }

        public static Rol Create(string matricula)
        {
            return new Profesional {Matricula = matricula};
        }

        public virtual void AgregarEspecialidad(Especialidad especialidad)
        {
            if (especialidad == null) throw new ArgumentNullException("especialidad");

            _especialidades.Add(especialidad);
        }

        public virtual void AgregarAgenda(DayOfWeek dia, TimeSpan duracionTurno, DateTime horarioDesde, DateTime horarioHasta, Consultorio consultorio, params Especialidad[] especialidades)
        {
            if (especialidades == null) throw new ArgumentNullException("especialidades");

            var agenda = new Agenda()
                             {
                                 Dia = dia,
                                 DuracionTurno = duracionTurno,
                                 HorarioDesde = horarioDesde,
                                 HorarioHasta = horarioHasta,
                                 Profesional = this,
                                 Consultorio = consultorio
                             };

            especialidades.ToList().ForEach(agenda.AgregarEspecialidad);

            _agendas.Add(agenda);
        }
    }
}