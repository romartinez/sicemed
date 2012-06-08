using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerProfesionalesDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerProfesionalesDropDownQuery : Query<IEnumerable<SelectListItem>>,
                                                     IObtenerProfesionalesDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var profesionales = SessionFactory.GetCurrentSession().QueryOver<Models.Roles.Profesional>()
                .JoinQueryOver(p => p.Persona)
                .OrderBy(p => p.Apellido).Asc.List();
            var result = MappingEngine.Map<IEnumerable<SelectListItem>>(profesionales).ToList();
            result.ForEach(s => s.Selected = (SelectedValue.HasValue && SelectedValue.Value.ToString() == s.Value));
            return result;
        }
    }
}