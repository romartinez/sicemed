using System;
using System.Linq;
using Iesi.Collections.Generic;

namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        private readonly ISet<Agenda> _agendas;
        private readonly ISet<Especialidad> _especialidades;

        protected Profesional()
        {
            _especialidades = new HashedSet<Especialidad>();
            _agendas = new HashedSet<Agenda>();
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

        public virtual void AgregarAgenda(Agenda agenda)
        {
            if (agenda == null) throw new ArgumentNullException("agenda");

            if (!agenda.EspecialidadesAtendidas.ToList().TrueForAll(x => _especialidades.Contains(x)))
                throw new ArgumentException(@"Alguna de las Especialidades seleccionadas no pertenece a las Especialidades del Profesional.", "especialidades");

            agenda.Profesional = this;
            _agendas.Add(agenda);
        }

        public virtual void AgregarAgenda(DayOfWeek dia, TimeSpan duracionTurno, DateTime horarioDesde, DateTime horarioHasta, Consultorio consultorio, params Especialidad[] especialidades)
        {
            if (especialidades == null) throw new ArgumentNullException("especialidades");

            if (!especialidades.ToList().TrueForAll(x => _especialidades.Contains(x)))
                throw new ArgumentException(@"Alguna de las Especialidades seleccionadas no pertenece a las Especialidades del Profesional.", "especialidades");

            if (horarioHasta < horarioDesde)
                throw new ArgumentOutOfRangeException("horarioHasta",
                                                      @"El Horario Desde es mayor al Horario Hasta de la Agenda.");

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