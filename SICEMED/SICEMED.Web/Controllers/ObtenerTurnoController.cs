using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt]
    public class ObtenerTurnoController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult ObtenerEspecialidades()
        {
            var session = SessionFactory.GetCurrentSession();
            var especialidades = session.QueryOver<Especialidad>()
                .OrderBy(x => x.Nombre).Asc
                .Future()
                .Select(x => new
                {
                    x.Id,
                    x.Nombre
                });
            return Json(especialidades, JsonRequestBehavior.AllowGet);
        }

        #region BuscarProfesional

        public virtual JsonResult BuscarProfesional(long? especialidadId, string nombre)
        {
            return especialidadId.HasValue ?
                BuscarProfesionalPorEspecialidad(especialidadId.Value, nombre)
                : BuscarProfesionalPorNombre(nombre);
        }

        private JsonResult BuscarProfesionalPorNombre(string nombre)
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Persona>()
                    .Where(
                        Restrictions.On<Persona>(p => p.Nombre).IsLike(nombre, MatchMode.Start)
                        || Restrictions.On<Persona>(p => p.SegundoNombre).IsLike(nombre, MatchMode.Start)
                        || Restrictions.On<Persona>(p => p.Apellido).IsLike(nombre, MatchMode.Start)
                    ).JoinQueryOver<Rol>(p => p.Roles)
                    .Where(r => r.GetType() == typeof(Profesional))
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();

            var profesionales = query.Select(ConverPersonaToProfesionalViewModel);

            return Json(profesionales, JsonRequestBehavior.AllowGet);
        }

        private JsonResult BuscarProfesionalPorEspecialidad(long especialidadId, string nombre)
        {
            var session = SessionFactory.GetCurrentSession();

            var especialidadConProfesionales = session.QueryOver<Especialidad>()
                .Where(e => e.Id == especialidadId)
                .JoinQueryOver<Profesional>(e => e.Profesionales)
                .JoinQueryOver(p => p.Persona)
                .Where(
                    Restrictions.On<Persona>(p => p.Nombre).IsLike(nombre, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.SegundoNombre).IsLike(nombre, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.Apellido).IsLike(nombre, MatchMode.Start)
                )
                .TransformUsing(Transformers.DistinctRootEntity)
                .Future();

            var profesionalesConEspecialidad = especialidadConProfesionales
                .SelectMany(e => e.Profesionales)
                .Select(p => p.Persona)
                .Select(ConverPersonaToProfesionalViewModel);

            return Json(profesionalesConEspecialidad, JsonRequestBehavior.AllowGet);
        }

        private static dynamic ConverPersonaToProfesionalViewModel(Persona p)
        {
            return new
            {
                p.As<Profesional>().Id,
                Foto = "/public/images/personal_128x128.png",
                Nombre = string.Format("{0}, {1} {2}", p.Apellido, p.Nombre, p.SegundoNombre),
                ProximoTurno = DateTime.Now,
                Especialidades = p.As<Profesional>().Especialidades.Select(e => e.Nombre)
            };
        }

        #endregion

        #region Obtener Agenda
        public virtual JsonResult ObtenerAgendaProfesional(long profesionalId)
        {
            var session = SessionFactory.GetCurrentSession();
            var clinica = session.QueryOver<Clinica>().FutureValue();
            var turnosProfesional = session.QueryOver<Turno>()
                .Where(t => t.FechaTurno > DateTime.Now.AddDays(-1) && t.FechaTurno < DateTime.Now.AddMonths(3))
                .JoinQueryOver(x => x.Profesional)
                .Where(p => p.Id == profesionalId)
                .Future();

            var agendaProfesional = session.QueryOver<Agenda>()
                .JoinQueryOver(a => a.Profesional)
                .Where(p => p.Id == profesionalId)
                .Future();

            //Creo los turnos libres 3 meses para adelante promedio 30 dias por mes
            var turnos = new List<dynamic>();
            for (var i = 0; i <= 3 * 30; i++)
            {
                var dia = DateTime.Now.AddDays(i);
                var agendaDia = agendaProfesional.FirstOrDefault(a => a.Dia == dia.DayOfWeek);

                if (agendaDia != null) turnos.AddRange(CalcularTurnos(dia, clinica.Value, agendaDia));
            }

            //Quito los turnos otorgados
            turnos.RemoveAll(x => turnosProfesional.Any(t => t.FechaTurno == x.FechaTurnoInicial));

            //Agrego los turnos otorgados
            turnos.AddRange(turnosProfesional.Select(t => new
                {
                    FechaTurnoInicial = t.FechaTurno,
                    FechaTurnoFinal = t.FechaTurno.AddMinutes(t.Agenda.DuracionTurno.TotalMinutes),
                    Consultorio = new
                    {
                        t.Consultorio.Id,
                        t.Consultorio.Nombre
                    },
                    t.Agenda,
                    Paciente = new
                    {
                        t.Paciente.Id,
                        t.Paciente.Persona.Nombre
                    }
                })
            );

            return Json(turnos, JsonRequestBehavior.AllowGet);
        }

        private List<dynamic> CalcularTurnos(DateTime dia, Clinica clinica, Agenda agendaDia)
        {
            var turnos = new List<dynamic>();
            var tiempoAtencion = agendaDia.HorarioHasta.Subtract(agendaDia.HorarioDesde);
            for (var minutes = 0; minutes < tiempoAtencion.TotalMinutes; minutes += (int)agendaDia.DuracionTurno.TotalMinutes)
            {
                var diaConHora = dia.SetTimeWith(agendaDia.HorarioDesde);
                turnos.Add(new
                {
                    FechaTurnoInicial = diaConHora.AddMinutes(minutes),
                    FechaTurnoFinal = diaConHora.AddMinutes(minutes).AddMinutes(agendaDia.DuracionTurno.TotalMinutes),
                    Agenda = agendaDia,
                    Consultorio = new
                    {
                        agendaDia.Consultorio.Id,
                        agendaDia.Consultorio.Nombre
                    }
                });
            }

            //Quito los turnos anteriores a que abra la clinica
            turnos.RemoveAll(x => x.FechaTurnoInicial <= dia.SetTimeWith(clinica.HorarioMatutinoDesde));

            //Quito los que son despues de que la clinica cerro
            var horarioCierreClinica = clinica.EsHorarioCorrido
                                           ? dia.SetTimeWith(clinica.HorarioMatutinoHasta) 
                                           : dia.SetTimeWith(clinica.HorarioVespertinoHasta.Value);

            turnos.RemoveAll(x => x.FechaTurnoInicial >= horarioCierreClinica);

            //Quito los turnos del mediodia si es horario cortado
            if (!clinica.EsHorarioCorrido)
            {
                turnos.RemoveAll(
                    x => x.FechaTurnoInicial >= dia.SetTimeWith(clinica.HorarioMatutinoHasta) 
                    && x.FechaTurnoInicial < dia.SetTimeWith(clinica.HorarioVespertinoDesde.Value));
            }

            return turnos;
        }
        #endregion

        #region Reservar Turno
        [HttpPost]
        public virtual JsonResult ReservarTurno(long profesionalId, DateTime fecha, long consultorioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var profesional = session.QueryOver<Profesional>().Where(p => p.Id == profesionalId).FutureValue();
            //TODO: Me falta la especialidad y la agenda (especialidad no estoy seguro que sea necesario.
            var turno = Turno.Create(fecha, User.As<Paciente>(), profesional.Value, profesional.Value.Especialidades.First(), Request.UserHostAddress);

            session.Save(turno);

            return Json(turno.Id);
        }
        #endregion

        public virtual ActionResult ImprimirComprobante(long turnoId)
        {
            throw new NotImplementedException();
        }
    }
}