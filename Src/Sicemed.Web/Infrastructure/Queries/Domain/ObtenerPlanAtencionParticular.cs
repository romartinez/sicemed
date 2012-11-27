using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerPlanAtencionParticular : IQuery<IEnumerable<SelectListItem>>
    {
        string SelectedValue { get; set; }
    }

    public class ObtenerPlanAtencionParticular : Query<IEnumerable<SelectListItem>>, IObtenerPlanAtencionParticular
    {
        public string SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var consultorios = SessionFactory.GetCurrentSession().QueryOver<Plan>()
                .OrderBy(x => x.Nombre).Asc.Future();
            return consultorios.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = x.Nombre == SelectedValue && x.Nombre == "Consulta Particular"
                });

        }
    }
}
