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

        [ValidateModelState]
        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult BusquedaPaciente(TurnosPorPacienteViewModel viewModel)
        {
            AppendLists(viewModel);
            var query = QueryFactory.Create<IBusquedaPacienteQuery>();
            query.Nombre = viewModel.SeleccionPaciente.Nombre;
            query.TipoDocumento = viewModel.SeleccionPaciente.TipoDocumento;
            query.NumeroDocumento = viewModel.SeleccionPaciente.NumeroDocumento;

            viewModel.SeleccionPaciente.PacientesEncontrados = query.Execute();
            viewModel.SeleccionPaciente.BusquedaEfectuada = true;

            return View("TurnosPorPaciente", viewModel);
        }

        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult TurnosPorPaciente(TurnosPorPacienteViewModel viewModel)
        {
            AppendLists(viewModel);
            return View(viewModel);
        }

        private void AppendLists(TurnosPorPacienteViewModel viewModel)
        {
            viewModel.SeleccionPaciente.TipoDocumentosHabilitados =
                GetTiposDocumentos(viewModel.SeleccionPaciente.TipoDocumento);
        }

        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult SeleccionPaciente(TurnosPorPacienteViewModel viewModel)
        {
            var paciente =
                SessionFactory.GetCurrentSession().Get<Paciente>(viewModel.SeleccionPaciente.PacienteSeleccionado.Id);

            viewModel.SeleccionPaciente.PacienteSeleccionado = MappingEngine.Map<InfoViewModel>(paciente);

            var query = QueryFactory.Create<IObtenerTurnosPorPacienteQuery>();
            query.FechaDesde = viewModel.Filters.Desde;
            query.FechaHasta = viewModel.Filters.Hasta;
            query.Filtro = viewModel.Filters.Filtro;
            query.PacienteId = paciente.Id;

            viewModel.Turnos = query.Execute();

            return View("TurnosPorPaciente", viewModel);
        }
    }
}