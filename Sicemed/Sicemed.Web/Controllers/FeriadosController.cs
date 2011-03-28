using System;
using System.Web.Mvc;
using Sicemed.Web.Plumbing;
using Sicemed.Web.Models;

namespace Sicemed.Web.Controllers
{   
    public class FeriadosController : BaseController
    {
        //
        // GET: /Feriado/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /Feriado/GridData

        public JsonResult GridData(int rows, int page)
        {
			var session = SessionFactory.GetCurrentSession();
			var count = session.QueryOver<Feriado>().RowCountInt64();
			var pageData = session.QueryOver<Feriado>().Skip((page - 1) * rows ).Take(rows).List();
			return Json(new {
				page,
				records = count,
				rows = pageData,
				total = Math.Ceiling((decimal) count / rows)
			}, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Feriado/Create

        [HttpPost]
        public void Create(Feriado feriado)
        {
            if (ModelState.IsValid)
            {
				var session = SessionFactory.GetCurrentSession();
				session.Save(feriado);
            }
        }
        
        //
        // POST: /Feriado/Edit/5

        [HttpPost]
        public void Edit(Feriado feriado)
        {
            if (ModelState.IsValid)
            {
				var session = SessionFactory.GetCurrentSession();
				session.Update(feriado);
            }
        }

        //
        // POST: /Feriado/Delete/5
		[HttpPost]
        public void Delete(long id)
        {
			var session = SessionFactory.GetCurrentSession();
			var feriado = session.QueryOver<Feriado>().Where(x => x.Id == id).SingleOrDefault();
			session.Delete(feriado);
        }

    }
}