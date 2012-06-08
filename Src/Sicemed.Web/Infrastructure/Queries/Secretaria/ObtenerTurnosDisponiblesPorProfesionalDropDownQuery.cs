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
        long? SelectedValue { get; set; }
    }

    public class ObtenerTurnosDisponiblesPorProfesionalDropDownQuery : Query<IEnumerable<SelectListItem>>, IObtenerTurnosDisponiblesPorProfesionalDropDownQuery
    {
        public long ProfesionalId { get; set; }
        public long EspecialidadId { get; set; }
        public long? SelectedValue { get; set; }

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
                    string.Join(", ", t.Agenda.EspecialidadesAtendidas.Select(e => e.Descripcion).ToArray()),
                    t.Consultorio.Descripcion
                    ),
                Value = t.FechaTurnoInicial.Ticks.ToString()
            }).ToList();

            result.ForEach(s => s.Selected = (SelectedValue.HasValue && SelectedValue.Value.ToString() == s.Value));

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