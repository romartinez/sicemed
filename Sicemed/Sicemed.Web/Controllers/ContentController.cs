using System.Linq;
using System.Text;
using System.Web.Mvc;
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

            if (pagina.Contenido.Contains("${ESPECIALIDADES}"))
            {
                if (Logger.IsInfoEnabled) Logger.InfoFormat("Encontrado el tag ${ESPECIALIDADES}, recuperando las especialidades.");

                var especialidades = ObtenerEspecialidadesConProfesionalesQuery.Execute();
                var sb = new StringBuilder();
                sb.Append("<ul>");
                foreach(var especialidad in especialidades)
                {
                    sb.AppendFormat("<li>{0}<ul>",especialidad.Nombre);                    
                    foreach(var profesional in especialidad.Profesionales)
                    {
                        sb.AppendFormat("<li>{0}, {1}</li>", profesional.Persona.Apellido, profesional.Persona.Nombre);
                    }
                    sb.Append("</ul></li>");
                }
                sb.Append("</ul>");

                var paginaOriginal = pagina;
                pagina = new Pagina() { Nombre = paginaOriginal.Nombre };
                pagina.Contenido = paginaOriginal.Contenido.Replace("${ESPECIALIDADES}", sb.ToString());
            }

            return View(pagina);
        }
    }
}