using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;

namespace Sicemed.Web.Infrastructure.Helpers
{
    public static class DomainExtensions
    {
        public static IEnumerable<SelectListItem> GetTiposDocumentos(int? selectedValue = null)
        {
            return Enumeration.GetAll<TipoDocumento>().Select(x => 
                new SelectListItem()
                    {
                        Value = x.Value.ToString(), 
                        Text = x.DisplayName,
                        Selected = selectedValue.HasValue && x.Value == selectedValue.Value
                    }); 
        }
    }
}