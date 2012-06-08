using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerEspecialidadesProfesionalDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long ProfesionalId { get; set; }
        string SelectedValue { get; set; }
        long GetEspecialidadId(string compositeId);
        long GetProfesionalId(string compositeId);
    }

    public class ObtenerEspecialidadesProfesionalDropDownQuery : Query<IEnumerable<SelectListItem>>,
                                                     IObtenerEspecialidadesProfesionalDropDownQuery
    {
        public long ProfesionalId { get; set; }
        public string SelectedValue { get; set; }
        
        public long GetEspecialidadId(string compositeId)
        {
            return Convert.ToInt64(compositeId.Split('_')[1]);
        }

        public long GetProfesionalId(string compositeId)
        {
            return Convert.ToInt64(compositeId.Split('_')[0]);
        }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var profesional = SessionFactory.GetCurrentSession().Get<Models.Roles.Profesional>(ProfesionalId);

            var result = profesional.Especialidades
                .OrderBy(e=>e.Nombre)
                .Select(e => new SelectListItem
            {
                Text = e.Nombre,
                Value = string.Format("{0}_{1}", ProfesionalId, e.Id)
            }).ToList();

            result.ForEach(s => s.Selected = (string.IsNullOrWhiteSpace(SelectedValue) && SelectedValue == s.Value));
            
            return result;
        }
    }
}