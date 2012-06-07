using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerProvinciasDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerProvinciasDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerProvinciasDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var provincias = SessionFactory.GetCurrentSession().QueryOver<Provincia>().OrderBy(x => x.Nombre).Asc.Future();
            return provincias.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = SelectedValue.HasValue && x.Id == SelectedValue.Value
                });
        }
    }
}