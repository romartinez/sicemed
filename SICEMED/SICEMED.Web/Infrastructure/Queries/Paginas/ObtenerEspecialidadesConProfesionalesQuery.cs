using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerEspecialidadesConProfesionalesQuery : IQuery<Especialidad> { }

    public class ObtenerEspecialidadesConProfesionalesQuery : Query<Especialidad>, IObtenerEspecialidadesConProfesionalesQuery
    {
        public override IEnumerable<Especialidad> Execute()
        {
            if (Logger.IsInfoEnabled) Logger.InfoFormat("Consultando las especialidades.");

            var session = SessionFactory.GetCurrentSession();

            return session.QueryOver<Especialidad>().Fetch(x=>x.Profesionales).Eager.List();
        }
    }
}