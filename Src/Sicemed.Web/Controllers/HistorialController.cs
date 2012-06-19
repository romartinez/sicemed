using System;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Historial;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.Historial;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Profesional))]
    [AuthorizeIt(typeof(Paciente))]
    [AuthorizeIt(typeof(Secretaria))]
    public class HistorialController : NHibernateController
    {
        [AuthorizeIt(typeof(Profesional))]
        public ActionResult Atenciones(AtencionesViewModel viewModel)
        {
            var session = SessionFactory.GetCurrentSession();
            var paciente = session.Load<Paciente>(viewModel.PacienteId);

            var query = QueryFactory.Create<IObtenerAtencionesQuery>();
            query.FechaDesde = viewModel.Filters.Desde;
            query.FechaHasta = viewModel.Filters.Hasta;
            query.Filtro = viewModel.Filters.Filtro;
            query.PacienteId = viewModel.PacienteId;
            query.ProfesionalId = User.As<Profesional>().Id;

            viewModel.Turnos = query.Execute();
            viewModel.PacienteNombre = paciente.Persona.NombreCompleto;

            return View(viewModel);
        }

        [AuthorizeIt(typeof(Paciente))]
        public ActionResult Turnos(TurnosPacienteViewModel viewModel)
        {
            var query = QueryFactory.Create<IObtenerTurnosPacienteQuery>();
            query.FechaDesde = viewModel.Filters.Desde;
            query.FechaHasta = viewModel.Filters.Hasta;
            query.Filtro = viewModel.Filters.Filtro;
            query.PacienteId = User.As<Paciente>().Id;

            var turnos = query.Execute();

            return View(turnos);
        }

        //[AuthorizeIt(typeof(Secretaria))]
        //public ActionResult TurnosPorPaciente(long? pacienteId = null, DateTime? desde = null, DateTime? hasta = null)
        //{
        //    if (!pacienteId.HasValue) return View(new TurnosPacienteViewModel());

        //    var query = QueryFactory.Create<IObtenerTurnosPacienteQuery>();
        //    query.FechaDesde = desde.HasValue ? desde.Value : DateTime.Now.AddMonths(-1 * DEFAULT_PREVIOUS_MONTHS);
        //    query.FechaHasta = hasta.HasValue ? hasta.Value : DateTime.Now;
        //    query.PacienteId = pacienteId.Value;

        //    var turnos = query.Execute();

        //    return View(turnos);
        //}

    }
}