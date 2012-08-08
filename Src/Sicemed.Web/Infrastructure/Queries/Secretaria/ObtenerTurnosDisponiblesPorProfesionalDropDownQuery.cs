using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Queries.ObtenerTurno;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerTurnosDisponiblesPorProfesionalDropDownQuery : IQuery<IEnumerable<SelectListItem>>
    {
        long ProfesionalId { get; set; }
        long EspecialidadId { get; set; }
        string SelectedValue { get; set; }
        DateTime GetFechaTurno(string compositeId);
        long GetConsultorioId(string compositeId);
    }

    public class ObtenerTurnosDisponiblesPorProfesionalDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerTurnosDisponiblesPorProfesionalDropDownQuery
    {
        public long ProfesionalId { get; set; }
        public long EspecialidadId { get; set; }
        public string SelectedValue { get; set; }
        
        public DateTime GetFechaTurno(string compositeId)
        {
            return new DateTime(Convert.ToInt64(compositeId.Split('_')[0]));
        }

        public long GetConsultorioId(string compositeId)
        {
            return Convert.ToInt64(compositeId.Split('_')[1]);
        }

        protected override IEnumerable<SelectListItem> CoreExecute()
        {
            var turnosQuery = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            turnosQuery.ProfesionalId = ProfesionalId;
            turnosQuery.EspecialidadId = EspecialidadId;

            var turnosDisponibles = turnosQuery.Execute();
            var result = turnosDisponibles.Select(t => new SelectListItem
            {
                Text = string.Format("{0:dd/MM/yy HH:mm} - {1} - {2}",
                    t.FechaTurnoInicial, 
                    string.Join(", ", t.EspecialidadesAtendidas.Select(e => e.Descripcion).ToArray()),
                    t.Consultorio.Descripcion
                    ),
                Value = string.Format("{0}_{1}", t.FechaTurnoInicial.Ticks, t.Consultorio.Id)
            }).ToList();

            result.ForEach(s => s.Selected = (!string.IsNullOrWhiteSpace(SelectedValue) && SelectedValue == s.Value));

            return result;
        }

        public override void ClearCache()
        {
            base.ClearCache();
            //Como uso esta query, la limpio en la cache
            var turnosQuery = QueryFactory.Create<IObtenerTurnosProfesionalQuery>();
            turnosQuery.ProfesionalId = ProfesionalId;
            turnosQuery.EspecialidadId = EspecialidadId;
            turnosQuery.ClearCache();
        }
    }
}