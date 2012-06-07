using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerPlanesPorObraSocialDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long ObraSocialId { get; set; }
        long? SelectedValue { get; set; }
    }

    public class ObtenerPlanesPorObraSocialDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerPlanesPorObraSocialDropDownQuery 
    {
        public long ObraSocialId { get; set; }
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var planes = SessionFactory.GetCurrentSession().QueryOver<Plan>().OrderBy(x => x.Nombre).Asc.
                JoinQueryOver(l => l.ObraSocial).Where(p => p.Id == ObraSocialId).Future();
            return planes.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = SelectedValue.HasValue && x.Id == SelectedValue.Value
                });

        }
    }
}