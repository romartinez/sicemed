using System;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Models;

namespace Sicemed.Web.Controllers
{   
    public class CalendariosController : BaseController
    {
        //
        // GET: /Calendario/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /Calendario/GridData

        public ActionResult GridData(int rows, int page)
        {
			var session = SessionFactory.GetCurrentSession();
			var count = session.QueryOver<Calendario>().RowCountInt64();
			var pageData = session.QueryOver<Calendario>().Fetch(x=>x.Feriados).Default.Skip((page - 1) * rows ).Take(rows).List();
            var settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return Json(new
            {
                page,
                records = count,
                rows = pageData,
                total = Math.Ceiling((decimal)count / rows)
            },"application/json",Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Calendario/Create

        [HttpPost]
        public void Create(Calendario calendario)
        {
            if (ModelState.IsValid)
            {
				var session = SessionFactory.GetCurrentSession();
				session.Save(calendario);
            }
        }
        
        //
        // POST: /Calendario/Edit/5

        [HttpPost]
        public void Edit(Calendario calendario)
        {
            if (ModelState.IsValid)
            {
				var session = SessionFactory.GetCurrentSession();
				session.Update(calendario);
            }
        }

        //
        // POST: /Calendario/Delete/5
		[HttpPost]
        public void Delete(long id)
        {
			var session = SessionFactory.GetCurrentSession();
			var calendario = session.QueryOver<Calendario>().Where(x => x.Id == id).SingleOrDefault();
			session.Delete(calendario);
        }

    }
}