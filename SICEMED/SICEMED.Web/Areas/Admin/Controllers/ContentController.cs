using System;
using System.Web.Mvc;
using System.Web.UI;
using Sicemed.Web.Infrastructure;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    public class ContentController : NHibernateController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public JsonResult List(long count, int page, int rows)
        {
            page--;
            var session = SessionFactory.GetCurrentSession();
            var query = session.QueryOver<Pagina>();

            var respuesta = new PaginableResponse<Pagina>();
            query.OrderBy(x => x.Nombre);
            respuesta.Rows = query.Take(rows).Skip(page * rows).Future<Pagina>();
            if (page == 0)
            {
                var queryCount = query.ToRowCountInt64Query().FutureValue<long>();
                respuesta.Records = queryCount.Value;
            }
            else
            {
                respuesta.Records = count;
            }
            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }


        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public JsonResult Nuevo(string oper, string nombre, int paginaId = 0)
        {
            if (!oper.Equals("add", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var pagina = new Pagina()
            {
                Nombre = nombre,
            };

            SessionFactory.GetCurrentSession().Save(pagina);

            return Json(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(long id, string oper, string nombre)
        {
            if (!oper.Equals("edit", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var session = SessionFactory.GetCurrentSession();

            var pagina = session.QueryOver<Pagina>().Where(x => x.Id == id).SingleOrDefault();

            if (pagina != null)
            {
                pagina.Nombre = nombre;
            }

            return Json(ResponseMessage.Success());
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(string id, string oper)
        {
            if (!oper.Equals("del", StringComparison.InvariantCultureIgnoreCase)) throw new ValidationErrorException();

            var idsSeleccionados = id.Split(',');
            var session = SessionFactory.GetCurrentSession();
            foreach (var idsSeleccionado in idsSeleccionados)
            {
                //session.Delete<Pagina>((int) idsSeleccionado);
            }

            return Json(ResponseMessage.Success());
        }

        [HttpGet]
        [AjaxHandleError]
        [OutputCache(Duration = 600, VaryByParam = "none", Location = OutputCacheLocation.ServerAndClient)]//10 minutes(10min*60sec)
        [ValidateAntiForgeryToken]
        public ActionResult ObtenerPaginas()
        {
            var paginas = SessionFactory.GetCurrentSession().QueryOver<Pagina>().List();
            return Json(paginas);
        }

    }
}
