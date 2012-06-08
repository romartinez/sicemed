﻿using System;
using System.Collections.Generic;
using System.Linq;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;

namespace Sicemed.Web.Infrastructure.Queries.ObtenerTurno
{
    public interface IObtenerTurnosProfesionalQuery : IQuery<IEnumerable<TurnoViewModel>>
    {
        long ProfesionalId { get; set; }
        long? EspecialidadId { get; set; }        
    }

    public class ObtenerTurnosProfesionalQuery : Query<IEnumerable<TurnoViewModel>>, IObtenerTurnosProfesionalQuery
    {
        public virtual long ProfesionalId { get; set; }
        public virtual long? EspecialidadId { get; set; }

        protected override IEnumerable<TurnoViewModel> CoreExecute()
        {
            var filtroEspecialidad = new Func<TurnoViewModel, bool>(x =>
                        (x.Especialidad != null && x.Especialidad.Id == EspecialidadId.Value)
                        || x.Agenda.EspecialidadesAtendidas.Any(e => e.Id == EspecialidadId.Value));

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
            var turnosProfesional = session.QueryOver<Turno>()
                .Where(t => t.FechaTurno > DateTime.Now.AddDays(-1) && t.FechaTurno < DateTime.Now.AddMonths(3))
                .JoinQueryOver(x => x.Profesional)
                .Where(p => p.Id == ProfesionalId)
                .JoinQueryOver(p => p.Especialidades)
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
                var agendaDia = agendaProfesional.FirstOrDefault(a => a.Dia == dia.DayOfWeek);

                if (agendaDia != null) turnos.AddRange(CalcularTurnos(dia, agendaDia));
            }

            //Quito los turnos otorgados
            turnos.RemoveAll(x => turnosProfesional.Any(t => t.FechaTurno == x.FechaTurnoInicial));

            //Agrego los turnos otorgados
            turnos.AddRange(turnosProfesional.Select(TurnoViewModel.Create));

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
                turnos.AddRange(TurnoViewModel.Create(diaConHora, agendaDia));
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