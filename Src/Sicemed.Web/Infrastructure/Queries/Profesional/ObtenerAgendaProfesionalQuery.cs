using Sicemed.Web.Models.ViewModels.Profesional;

namespace Sicemed.Web.Infrastructure.Queries.Profesional
{
    public interface IObtenerAgendaProfesionalQuery : IQuery<AgendaProfesionalViewModel>
    {
        long ProfesionalId { get; set; }
    }

    public class ObtenerAgendaProfesionalQuery : Query<AgendaProfesionalViewModel>, IObtenerAgendaProfesionalQuery 
    {
        public virtual long ProfesionalId { get; set; }

        protected override AgendaProfesionalViewModel CoreExecute()
        {
            return null;
        }
    }
}