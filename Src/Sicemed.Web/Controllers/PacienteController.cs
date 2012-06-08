﻿using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Paciente;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Controllers
{
    [AuthorizeIt(typeof(Paciente))]
    public class PacienteController : NHibernateController
    {

        public ActionResult Agenda()
        {
            var query = QueryFactory.Create<IObtenerAgendaPacienteQuery>();
            query.PacienteId = User.As<Paciente>().Id;
            var viewModel = query.Execute();
            return View(viewModel);             
        }
    }
}