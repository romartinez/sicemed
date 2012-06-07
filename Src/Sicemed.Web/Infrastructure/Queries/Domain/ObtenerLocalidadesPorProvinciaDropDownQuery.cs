using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerLocalidadesPorProvinciaDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long ProvinciaId { get; set; }
        long? SelectedValue { get; set; }
    }

    public class ObtenerLocalidadesPorProvinciaDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerLocalidadesPorProvinciaDropDownQuery
    {
        public long ProvinciaId { get; set; }
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var localidades = SessionFactory.GetCurrentSession().QueryOver<Localidad>().OrderBy(x => x.Nombre).Asc.
                JoinQueryOver(l => l.Provincia).Where(p => p.Id == ProvinciaId).Future();
            return localidades.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = SelectedValue.HasValue && x.Id == SelectedValue.Value
                });

        }
    }
}