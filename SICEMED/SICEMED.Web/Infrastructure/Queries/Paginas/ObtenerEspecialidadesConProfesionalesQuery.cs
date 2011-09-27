using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerEspecialidadesConProfesionalesQuery : IQuery<IEnumerable<Especialidad>> { }

    public class ObtenerEspecialidadesConProfesionalesQuery : Query<IEnumerable<Especialidad>>, IObtenerEspecialidadesConProfesionalesQuery
    {
        public override IEnumerable<Especialidad> CoreExecute()
        {
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Consultando las especialidades.");

            var session = SessionFactory.GetCurrentSession();

            return session.QueryOver<Especialidad>().Fetch(x=>x.Profesionales).Eager.List();
        }
    }
}