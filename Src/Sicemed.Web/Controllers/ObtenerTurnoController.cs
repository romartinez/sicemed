using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using SICEMED.Web;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;
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

        public JsonResult ObtenerEspecialidades()
        {
            //Lo dejo asi sino tengo que tocar el JS.
            var result = GetEspecialidades().Select(x => new {Id = x.Value, Nombre = x.Text});
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region BuscarProfesional

        public virtual JsonResult BuscarProfesional(long? especialidadId, string nombre)
        {
            var query = QueryFactory.Create<IObtenerProfesionalPorEspecialidadONombreQuery>();
            query.EspecialidadId = especialidadId;
            query.Profesional = nombre;
            var profesionales = query.Execute();

            //Filtro por los que tienen turnos libres
            var profesionalesConProximoTurno = new List<BusquedaProfesionalViewModel>();
            foreach (var p in profesionales)
            {
                p.ProximoTurnoLibre = ObtenerProximoTurnoLibre(p.Id, especialidadId);
                profesionalesConProximoTurno.Add(p);
            }

            return Json(profesionalesConProximoTurno, JsonRequestBehavior.AllowGet);
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

        private IEnumerable<TurnoViewModel> ObtenerAgenda(long profesionalId, long? especialidadId = null)
        {
            var query = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            query.ProfesionalId = profesionalId;
            query.EspecialidadId = especialidadId;
            return query.Execute();
        }
        #endregion

        #region Reservar Turno
        [HttpPost]
        public virtual JsonResult ReservarTurno(long profesionalId, DateTime fecha, long especialidadId, long consultorioId)
        {
            var session = SessionFactory.GetCurrentSession();
            var profesional = session.Get<Profesional>(profesionalId);
            var especialidad = session.Get<Especialidad>(especialidadId);
            var consultorio = session.Get<Consultorio>(consultorioId);

            //TODO: Validar las políticas de la clínica y comportamiento del usuario

            var turno = Turno.Create(fecha, User.As<Paciente>(), profesional, especialidad, consultorio, Request.UserHostAddress);

            session.Save(turno);

            //Update del cache
            var query = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            query.ProfesionalId = profesionalId;
            query.ClearCache();

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