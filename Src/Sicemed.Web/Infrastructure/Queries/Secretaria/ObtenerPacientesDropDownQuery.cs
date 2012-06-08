using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models.Roles;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerPacientesDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long? SelectedValue { get; set; }
    }

    public class ObtenerPacientesDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerPacientesDropDownQuery
    {
        public long? SelectedValue { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var pacientes = SessionFactory.GetCurrentSession().QueryOver<Paciente>()
                .JoinQueryOver(p=>p.Persona)
                .OrderBy(p => p.Apellido).Asc.List();
            var result = MappingEngine.Map<IEnumerable<SelectListItem>>(pacientes).ToList();
            result.ForEach(s => s.Selected = (SelectedValue.HasValue && SelectedValue.Value.ToString() == s.Value));
            return result;
        }
    }
}