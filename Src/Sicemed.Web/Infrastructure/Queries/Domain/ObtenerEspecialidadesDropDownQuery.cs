using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerEspecialidadesDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long[] SelectedValues { get; set; }
    }

    public class ObtenerEspecialidadesDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerEspecialidadesDropDownQuery
    {
        public long[] SelectedValues { get; set; }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var selectedIds = SelectedValues ?? new long[] { };
            var especialidades = SessionFactory.GetCurrentSession().QueryOver<Especialidad>().OrderBy(x => x.Nombre).Asc.Future();
            return especialidades.Select(x =>
                new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre,
                    Selected = selectedIds.Contains(x.Id)
                });
        }
    }
}