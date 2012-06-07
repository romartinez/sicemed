using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerConsultoriosDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerConsultoriosDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerConsultoriosDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var consultorios = SessionFactory.GetCurrentSession().QueryOver<Consultorio>()
                .OrderBy(x => x.Nombre).Asc.Future();
            return consultorios.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = SelectedValue.HasValue && x.Id == SelectedValue.Value
                });

        }
    }
}