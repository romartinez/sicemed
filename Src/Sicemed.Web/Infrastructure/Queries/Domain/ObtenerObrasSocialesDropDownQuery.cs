using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerObrasSocialesDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerObrasSocialesDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerObrasSocialesDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var obrasSociales = SessionFactory.GetCurrentSession().QueryOver<ObraSocial>().OrderBy(x => x.RazonSocial).Asc.Future();
            return obrasSociales.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.RazonSocial,
                    Selected = SelectedValue.HasValue && x.Id == SelectedValue.Value
                });            
        }
    }
}