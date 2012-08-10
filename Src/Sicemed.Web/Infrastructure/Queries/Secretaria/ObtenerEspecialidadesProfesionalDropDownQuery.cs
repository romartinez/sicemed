using System.Collections.Generic;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerEspecialidadesProfesionalQuery : IQuery<IEnumerable<InfoViewModel>>
    {
        long ProfesionalId { get; set; }
    }

    public class ObtenerEspecialidadesProfesionalQuery : Query<IEnumerable<InfoViewModel>>, IObtenerEspecialidadesProfesionalQuery
    {
        public long ProfesionalId { get; set; }

        protected override IEnumerable<InfoViewModel> CoreExecute()
        {
            var profesional = SessionFactory.GetCurrentSession().Get<Models.Roles.Profesional>(ProfesionalId);

            return MappingEngine.Map<IEnumerable<InfoViewModel>>(profesional.Especialidades);
        }
    }
}