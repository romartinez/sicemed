using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.Queries.ListadoProfesionales
{
    //public interface IObtenerProfesionalesQuery : IQuery<IEnumerable<List<Models.Roles.Profesional>>>
    //{
    //    long? SelectedValue { get; set; }
    //}

    //public class ObtenerProfesionalesQuery : Query<IEnumerable<SelectListItem>>,IObtenerProfesionalesQuery
    //{
    //    public long? SelectedValue { get; set; }

    //    protected override IEnumerable<SelectListItem> CoreExecute()
    //    {
    //        var profesionales = SessionFactory.GetCurrentSession().QueryOver<Models.Roles.Profesional>()
    //            .JoinQueryOver(p => p.Persona)
    //            .OrderBy(p => p.Apellido).Asc.List();
    //        var result = MappingEngine.Map<IEnumerable<SelectListItem>>(profesionales).ToList();

    //        if (SelectedValue.HasValue)
    //        {
    //            result.ForEach(s => s.Selected = (SelectedValue.HasValue && SelectedValue.Value.ToString() == s.Value));
    //        }
    //        return result;
    //    }
    //}
}