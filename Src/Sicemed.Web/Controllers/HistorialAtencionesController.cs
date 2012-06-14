using System;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.HistorialAtenciones;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Paciente))]
    [AuthorizeIt(typeof(Profesional))]
    public class HistorialAtencionesController : NHibernateController
    {
        public ActionResult Index(long pacienteId, DateTime? desde = null, DateTime? hasta = null)
        {
            var query = QueryFactory.Create<IObtenerHistorialAtencionesQuery>();
            query.FechaDesde = desde.HasValue ? desde.Value : DateTime.Now.AddMonths(-3);
            query.FechaHasta = hasta.HasValue ? hasta.Value : DateTime.Now;
            query.PacienteId = pacienteId;

            if (User.IsInRole<Profesional>() && !User.IsInRole<Paciente>())
                query.ProfesionalId = User.As<Profesional>().Id;

            var turnos = query.Execute();

            return View(turnos);
        }
    }
}