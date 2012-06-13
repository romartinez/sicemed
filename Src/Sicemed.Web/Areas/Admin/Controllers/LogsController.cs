using System;
using System.Web.Mvc;
using NHibernate.Transform;
using Sicemed.Web.Areas.Admin.Models;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class LogsController : NHibernateController
    {
        public virtual ActionResult Index()
        {
            var viewModel = new RangoFechasViewModel();
            viewModel.Dasde = DateTime.Now.AddDays(-7).ToMidnigth();
            viewModel.Hasta = DateTime.Now.AddDays(1).ToMidnigth();
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows, RangoFechasViewModel extra = null)
        {
            page--;

            var session = SessionFactory.GetCurrentSession();
            var query = session.CreateSQLQuery(@"
                                                SELECT [Id]
                                                      ,[Date]
                                                      ,[Thread]
                                                      ,[UserId]
                                                      ,[UserIp]
                                                      ,[SessionId]
                                                      ,[Level]
                                                      ,[Logger]
                                                      ,[Message]
                                                      ,[Exception]
                                                  FROM [Log]
                                                  ORDER BY [Date] DESC
                                               ").SetResultTransformer(Transformers.AliasToEntityMap)
                                                .SetFetchSize(rows)
                                                .SetFirstResult(page * rows);
            var respuesta = new PaginableResponse();

            if (page == 0)
            {
                var queryCount = session.CreateSQLQuery(@"SELECT COUNT(*) FROM [Log]").UniqueResult();
                respuesta.Records = Convert.ToInt64(queryCount);
            }
            else
            {
                respuesta.Records = count;
            }
            respuesta.Rows = query.List();

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult Get(long id)
        {
            var session = SessionFactory.GetCurrentSession();
            var log = session.CreateSQLQuery(@"
                                                SELECT [Id]
                                                      ,[Date]
                                                      ,[Thread]
                                                      ,[UserId]
                                                      ,[UserIp]
                                                      ,[SessionId]
                                                      ,[Level]
                                                      ,[Logger]
                                                      ,[Message]
                                                      ,[Exception]
                                                  FROM [Log]
                                                  WHERE [Id] = :id
                                               ").SetParameter("id", id)
                                                .SetResultTransformer(Transformers.AliasToEntityMap)
                                                .UniqueResult();
            return log == null ? Json(HttpNotFound()) : Json(log);
        }
    }
}