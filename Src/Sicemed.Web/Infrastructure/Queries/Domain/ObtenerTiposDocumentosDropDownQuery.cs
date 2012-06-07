using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerTiposDocumentosDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerTiposDocumentosDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerTiposDocumentosDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            return Enumeration.GetAll<TipoDocumento>().Select(x =>
                new SelectListItem()
                {
                    Value = x.Value.ToString(),
                    Text = x.DisplayName,
                    Selected = SelectedValue.HasValue && x.Value == SelectedValue.Value
                });
        }
    }
}