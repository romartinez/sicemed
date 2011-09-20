using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using RazorEngine;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Queries.Paginas;
using Sicemed.Web.Models;

namespace Sicemed.Web.Controllers
{
    public class ContentController : NHibernateController
    {
        public IObtenerEspecialidadesConProfesionalesQuery ObtenerEspecialidadesConProfesionalesQuery { get; set; }

        public virtual ActionResult Index(long id = 0)
        {
            var session = SessionFactory.GetCurrentSession();

            var pagina = id == 0
                             ? session.QueryOver<Pagina>().Where(p => p.Padre == null).Take(1).Future().First()
                             : session.Get<Pagina>(id);

            if (pagina == null) return View("NotFound");

            var model = new
            {
                Especialidades = new Lazy<IEnumerable<Especialidad>>(() => ObtenerEspecialidadesConProfesionalesQuery.Execute())
            };

            var paginaARenderizar = new Pagina { Nombre = pagina.Nombre };

            try
            {
                paginaARenderizar.Contenido = Razor.Parse(pagina.Contenido, model);
            }
            catch (RuntimeBinderException ex)
            {
                ViewBag.Nombre = paginaARenderizar.Nombre;
                ViewBag.Template = pagina.Contenido;
                ViewBag.Error = ex.ToString();
                return View("TemplateError");
            }

            return View(paginaARenderizar);
        }
    }
}