using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using NHibernate.Criterion;
using NHibernate.Transform;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;
using iTextSharp.text.pdf;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Paciente))]
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

            var profesionales = especialidadId.HasValue ?
                BuscarProfesionalPorEspecialidad(especialidadId.Value, nombre)
                : BuscarProfesionalPorNombre(nombre);

            var profesionalesConProximoTurno = new List<BusquedaProfesionalViewModel>();
            foreach (var p in profesionales)
            {
                p.ProximoTurnoLibre = ObtenerProximoTurnoLibre(p.Id, especialidadId);
                profesionalesConProximoTurno.Add(p);
            }

            return Json(profesionalesConProximoTurno, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<BusquedaProfesionalViewModel> BuscarProfesionalPorNombre(string nombre)
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

            return query.Select(ConverPersonaToProfesionalViewModel);
        }

        private IEnumerable<BusquedaProfesionalViewModel> BuscarProfesionalPorEspecialidad(long especialidadId, string nombre)
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

            return especialidadConProfesionales
                .SelectMany(e => e.Profesionales)
                .Select(p => p.Persona)
                .Select(ConverPersonaToProfesionalViewModel);
        }

        private static BusquedaProfesionalViewModel ConverPersonaToProfesionalViewModel(Persona persona)
        {
            return BusquedaProfesionalViewModel.Create(persona);
        }

        private TurnoViewModel ObtenerProximoTurnoLibre(long profesionalId, long? especialidadId = null)
        {
            var turnos = ObtenerAgenda(profesionalId, especialidadId);
            return turnos.FirstOrDefault();
        }
        #endregion

        #region Obtener Agenda
        public virtual JsonResult ObtenerAgendaProfesional(long profesionalId, long? especialidadId = null)
        {
            return Json(ObtenerAgenda(profesionalId, especialidadId), JsonRequestBehavior.AllowGet);
        }

        private List<TurnoViewModel> ObtenerAgenda(long profesionalId, long? especialidadId = null)
        {
            var filtroEspecialidad = new Func<TurnoViewModel, bool>(x =>
                        (x.Especialidad != null && x.Especialidad.Id == especialidadId.Value)
                        || x.Agenda.EspecialidadesAtendidas.Any(e => e.Id == especialidadId.Value));

            var cacheKey = String.Format("TURNOS_{0}", profesionalId);
            var cached = Cache.Get<List<TurnoViewModel>>(cacheKey);
            if (cached != null)
            {
                var turnosCached = cached.Clone();
                if (especialidadId.HasValue)
                {
                    turnosCached = turnosCached.Where(filtroEspecialidad).ToList();
                }
                return turnosCached;
            }

            var session = SessionFactory.GetCurrentSession();
            var turnosProfesional = session.QueryOver<Turno>()
                .Where(t => t.FechaTurno > DateTime.Now.AddDays(-1) && t.FechaTurno < DateTime.Now.AddMonths(3))
                .JoinQueryOver(x => x.Profesional)
                .Where(p => p.Id == profesionalId)
                .JoinQueryOver(p => p.Especialidades)
                .Future();

            var agendaProfesional = session.QueryOver<Agenda>()
                .JoinQueryOver(a => a.Profesional)
                .Where(p => p.Id == profesionalId)
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
            return !especialidadId.HasValue ? turnos : turnos.Where(filtroEspecialidad).ToList();
        }

        private List<TurnoViewModel> CalcularTurnos(DateTime dia, Agenda agendaDia)
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
        #endregion

        #region Reservar Turno
        [HttpPost]
        public virtual JsonResult ReservarTurno(long profesionalId, DateTime fecha, long especialidadId, long agendaId)
        {
            var session = SessionFactory.GetCurrentSession();
            var profesional = session.Get<Profesional>(profesionalId);
            var especialidad = session.Get<Especialidad>(especialidadId);
            var agenda = session.Get<Agenda>(agendaId);

            //TODO: Validar las políticas de la clínica y comportamiento del usuario

            var turno = Turno.Create(fecha, User.As<Paciente>(), profesional, especialidad, Request.UserHostAddress, agenda);

            session.Save(turno);

            return Json(ReservaTurnoViewModel.Create(turno));
        }
        #endregion

        public virtual void ImprimirComprobante(long turnoId)
        {
            var turno = SessionFactory.GetCurrentSession().Load<Turno>(turnoId);
            //Valido que sea del paciente actual
            if (turno.Paciente.Id != User.As<Paciente>().Id)
                throw new SecurityException("El usuario actual no es al que se le otorgó el turno.");

            // Read the template            
            var reader = new PdfReader(Server.MapPath("~/Reports/ComprobanteTurno.pdf"));

            // Writes the modified template to http response
            this.HttpContext.Response.Clear();
            this.HttpContext.Response.ContentType = "application/pdf";
            var stamper = new PdfStamper(reader, this.HttpContext.Response.OutputStream);

            // Retrieve the PDF form fields defined in the template
            var form = stamper.AcroFields;

            // Set values for the fields
            form.SetField("Fecha", turno.FechaTurno.ToString());
            form.SetField("Profesional", turno.Profesional.Persona.NombreCompleto);
            form.SetField("Especialidad", turno.Especialidad.Nombre);
            form.SetField("Paciente", turno.Paciente.Persona.NombreCompleto);
            form.SetField("Consultorio", turno.Consultorio.Nombre);

            // Setting this to true to make the document read-only
            stamper.FormFlattening = true;

            //Close the stamper instance
            stamper.Close();
        }
    }
}