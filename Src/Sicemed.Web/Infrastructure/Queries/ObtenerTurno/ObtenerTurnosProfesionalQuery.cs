using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Transform;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;

namespace Sicemed.Web.Infrastructure.Queries.ObtenerTurno
{
    public interface IObtenerTurnosProfesionalQuery : IQuery<IEnumerable<TurnoViewModel>>
    {
        long ProfesionalId { get; set; }
        long? EspecialidadId { get; set; }
        bool AgregarOtorgados { get; set; }
    }

    public class ObtenerTurnosProfesionalQuery : Query<IEnumerable<TurnoViewModel>>, IObtenerTurnosProfesionalQuery
    {
        public virtual long ProfesionalId { get; set; }
        public virtual long? EspecialidadId { get; set; }
        public virtual bool AgregarOtorgados { get; set; }

        protected override IEnumerable<TurnoViewModel> CoreExecute()
        {
            var filtroEspecialidad = new Func<TurnoViewModel, bool>(x =>
                        (x.EspecialidadesAtendidas.Any(e => e.Id == EspecialidadId.Value)));

            var cacheKey = GetProfesionalCacheKey();
            var cached = Cache.Get<List<TurnoViewModel>>(cacheKey);
            if (cached != null)
            {
                var turnosCached = cached.Clone();
                if (EspecialidadId.HasValue)
                {
                    turnosCached = turnosCached.Where(filtroEspecialidad).ToList();
                }
                return turnosCached;
            }

            var session = SessionFactory.GetCurrentSession();
            var maximoTurnosFuturos = DateTime.Now.AddMonths(3);
            var lunesDeEstaSemana = DateTime.Now.StartOfWeek(DayOfWeek.Tuesday).ToMidnigth();
            var turnosProfesional = session.QueryOver<Turno>()
                .Fetch(x => x.Paciente).Eager
                .Where(t => t.FechaTurno > lunesDeEstaSemana && t.FechaTurno < maximoTurnosFuturos)
                .WhereNot(t => t.Estado == EstadoTurno.Cancelado)
                .JoinQueryOver(x => x.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .JoinQueryOver(p => p.Especialidades)
                .TransformUsing(Transformers.DistinctRootEntity)
                .Future();

            var agendaProfesional = session.QueryOver<Agenda>()
                .JoinQueryOver(a => a.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .JoinQueryOver(p => p.Especialidades).Future();

            //Creo los turnos libres 3 meses para adelante promedio 30 dias por mes
            var turnos = new List<TurnoViewModel>();
            for (var i = 0; i <= 3 * 30; i++)
            {
                var dia = DateTime.Now.AddDays(i);
                foreach (var agendaDia in agendaProfesional.Where(a => a.Dia == dia.DayOfWeek))
                {
                    turnos.AddRange(CalcularTurnos(dia, agendaDia));
                }
            }

            //Quito los turnos otorgados
            turnos.RemoveAll(turnoLibre => turnosProfesional.Any(turnoLibre.SeSolapaConTurno));

            if (AgregarOtorgados)
            {
                var turnosOtorgados = MappingEngine.Map<List<TurnoViewModel>>(turnosProfesional);
                turnos.AddRange(turnosOtorgados);
            }

            //Quito los feriados y ausencias
            var feriados = session.QueryOver<Feriado>()
                .Where(x => x.Fecha >= lunesDeEstaSemana && x.Fecha <= maximoTurnosFuturos)
                .Future();

            var ausencias = session.QueryOver<Ausencia>()
                .Where(x => x.Fecha >= lunesDeEstaSemana && x.Fecha <= maximoTurnosFuturos)
                .Future();

            var feriadosSoloFecha = feriados.Select(x => x.Fecha.ToMidnigth());
            turnos.RemoveAll(t => feriadosSoloFecha.Contains(t.FechaTurnoInicial.ToMidnigth()));

            turnos.RemoveAll(t => ausencias.Any(a => a.EnPeriodoDeAusencia(t.FechaTurnoInicial)
                || a.EnPeriodoDeAusencia(t.FechaTurnoFinal)));

            Cache.Add(cacheKey, turnos);

            //Filtro por especialidad
            return !EspecialidadId.HasValue ? turnos : turnos.Where(filtroEspecialidad).ToList();
        }

        public override void ClearCache()
        {
            base.ClearCache();
            Cache.Remove(GetProfesionalCacheKey());
        }

        private string GetProfesionalCacheKey()
        {
            return string.Format("TURNOS_CACHE_{0}", ProfesionalId);
        }

        private IEnumerable<TurnoViewModel> CalcularTurnos(DateTime dia, Agenda agendaDia)
        {
            var turnos = new List<TurnoViewModel>();
            var tiempoAtencion = agendaDia.HorarioHasta.Subtract(agendaDia.HorarioDesde);
            for (var minutes = 0; minutes < tiempoAtencion.TotalMinutes; minutes += (int)agendaDia.DuracionTurno.TotalMinutes)
            {
                var diaConHora = dia.SetTimeWith(agendaDia.HorarioDesde).AddMinutes(minutes);

                turnos.Add(TurnoViewModel.Create(diaConHora, agendaDia));
            }

            //Quito los turnos anteriores a que abra la clinica
            turnos.RemoveAll(x => x.FechaTurnoInicial <= dia.SetTimeWith(MvcApplication.Clinica.HorarioMatutinoDesde));

            //Quito los que son despues de que la clinica cerro
            var horarioCierreClinica = MvcApplication.Clinica.EsHorarioCorrido
                                           ? dia.SetTimeWith(MvcApplication.Clinica.HorarioMatutinoHasta)
                                           : dia.SetTimeWith(MvcApplication.Clinica.HorarioVespertinoHasta.Value);

            turnos.RemoveAll(x => x.FechaTurnoInicial >= horarioCierreClinica);

            //Quito los turnos del mediodia si es horario cortado
            if (!MvcApplication.Clinica.EsHorarioCorrido)
            {
                turnos.RemoveAll(
                    x => x.FechaTurnoInicial >= dia.SetTimeWith(MvcApplication.Clinica.HorarioMatutinoHasta)
                    && x.FechaTurnoInicial < dia.SetTimeWith(MvcApplication.Clinica.HorarioVespertinoDesde.Value));
            }

            return turnos;
        }

    }
}