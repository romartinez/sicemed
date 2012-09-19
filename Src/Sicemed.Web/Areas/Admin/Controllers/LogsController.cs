using System;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Transform;
using Sicemed.Web.Areas.Admin.Models.Logs;
using Sicemed.Web.Infrastructure.Attributes.Filters;
using Sicemed.Web.Infrastructure.Controllers;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Areas.Admin.Controllers
{
    [AuthorizeIt(typeof(Administrador))]
    public class LogsController : NHibernateController
    {
        public virtual ActionResult Index(LogsSearchFiltersViewModel viewModel = null)
        {
            if (viewModel == null) 
                viewModel = new LogsSearchFiltersViewModel() { Desde = DateTime.Now.AddDays(-7).ToMidnigth() };
            return View(viewModel);
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult List(long count, int page, int rows, LogsSearchFiltersViewModel searchFilters)
        {
            page--;

            var sql = AppendWhere(@"SELECT [Id] ,[Date] ,[Thread] ,[UserId] ,[UserIp] ,[SessionId] ,[Level] ,[Logger] "
                + " FROM [Log]", searchFilters)
                + @" ORDER BY [Date] DESC";

            var session = SessionFactory.GetCurrentSession();
            var query = session.CreateSQLQuery(sql).SetResultTransformer(Transformers.AliasToEntityMap)
                .SetFetchSize(rows)
                .SetFirstResult(page * rows);

            query = AppendParameters(query, searchFilters);

            var respuesta = new PaginableResponse();

            var sqlCount = AppendWhere(@"SELECT COUNT(*) FROM [Log]", searchFilters);
            var queryCount = session.CreateSQLQuery(sqlCount) as IQuery;
            queryCount = AppendParameters(queryCount, searchFilters);
            var queryCountResult = queryCount.UniqueResult();
            respuesta.Records = Convert.ToInt64(queryCountResult);
            respuesta.Rows = query.List();

            respuesta.Page = ++page;
            respuesta.Total = (long)Math.Ceiling(respuesta.Records / (double)rows);
            return Json(respuesta);
        }

        private IQuery AppendParameters(IQuery query, LogsSearchFiltersViewModel searchFilters)
        {
            query = query.SetDateTime("Desde", searchFilters.Desde);
            query = query.SetDateTime("Hasta", searchFilters.Hasta);
            if (!string.IsNullOrWhiteSpace(searchFilters.LogLevelSeleccionado))
                query = query.SetString("Level", searchFilters.LogLevelSeleccionado);
            if (!string.IsNullOrWhiteSpace(searchFilters.Filtro))
                query = query.SetString("Filtro", searchFilters.Filtro + "%");
            return query;
        }

        private string AppendWhere(string sql, LogsSearchFiltersViewModel searchFilters)
        {
            sql += " WHERE [Date] BETWEEN :Desde AND :Hasta";
            if (!string.IsNullOrWhiteSpace(searchFilters.LogLevelSeleccionado))
                sql += @" AND [Level] = :Level";
            if (!string.IsNullOrWhiteSpace(searchFilters.Filtro))
                sql += @" AND ([UserId] LIKE :Filtro OR [UserIP] LIKE :Filtro OR [Logger] LIKE :Filtro OR [Message] LIKE :Filtro OR [Exception] LIKE :Filtro)";
            return sql;
        }

        [HttpPost]
        [AjaxHandleError]
        [ValidateAntiForgeryToken]
        public virtual JsonResult Get(long id)
        {
            var session = SessionFactory.GetCurrentSession();
            var log = session.CreateSQLQuery(@"SELECT * FROM [Log] WHERE [Id] = :id").SetParameter("id", id)
                                                .SetResultTransformer(Transformers.AliasToEntityMap)
                                                .UniqueResult();
            return log == null ? Json(HttpNotFound()) : Json(log);
        }
    }
}