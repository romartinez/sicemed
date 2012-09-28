using System;
using System.Web.Mvc;
using NHibernate;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Busqueda;
using Sicemed.Web.Infrastructure.Queries.Historial;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;
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

        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult TurnosPorPaciente(TurnosPorPacienteViewModel viewModel)
        {
            if (!viewModel.BusquedaEfectuada)
            {
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                return View(new TurnosPorPacienteViewModel());
            }

            if (!ModelState.IsValid) return View(viewModel);

            var query = QueryFactory.Create<IObtenerTurnosPorPacienteQuery>();
            query.FechaDesde = viewModel.Desde;
            query.FechaHasta = viewModel.Hasta;
            query.Filtro = viewModel.Filtro;
            query.PacienteId = viewModel.PacienteId.Value;

            viewModel.Turnos = query.Execute();

            return View(viewModel);
        }
    }
}