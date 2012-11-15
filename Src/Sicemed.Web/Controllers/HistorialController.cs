﻿using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
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

        [HttpGet]
        [AuthorizeIt(typeof(Profesional))]
        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult SeleccionPaciente()
        {
            return View(new SeleccionPacienteViewModel());
        }

        [HttpPost]
        [ValidateModelState]
        [AuthorizeIt(typeof(Profesional))]
        [AuthorizeIt(typeof(Secretaria))]
        public ActionResult SeleccionPaciente(SeleccionPacienteViewModel viewModel)
        {
            return RedirectToAction("TurnosPorPaciente", new { pacienteId = viewModel.PacienteId });
        }

        public ActionResult TurnosPorPaciente(long? pacienteId = null)
        {
            //Si es sólo paciente, puede ver sus atenciones únicamente
            if (User.IsInRole<Paciente>() && User.Roles.Count == 1) pacienteId = User.As<Paciente>().Id;

            if (!pacienteId.HasValue) return RedirectToAction("SeleccionPaciente");

            var viewModel = new TurnosPorPacienteViewModel();
            var paciente = SessionFactory.GetCurrentSession().Get<Paciente>(pacienteId);
            if (paciente == null) return HttpNotFound();


            viewModel.PacienteSeleccionado = MappingEngine.Map<InfoViewModel>(paciente);

            var query = QueryFactory.Create<IObtenerTurnosPorPacienteQuery>();
            query.FechaDesde = viewModel.Filters.Desde;
            query.FechaHasta = viewModel.Filters.Hasta;
            query.Filtro = viewModel.Filters.Filtro;
            query.PacienteId = paciente.Id;

            viewModel.Turnos = query.Execute();

            return View(viewModel);
        }
    }
}