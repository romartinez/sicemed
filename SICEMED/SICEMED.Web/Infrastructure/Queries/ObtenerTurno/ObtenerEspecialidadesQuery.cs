using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.ObtenerTurno
{
    public interface IObtenerEspecialidadesQuery : IQuery<IEnumerable<Especialidad>>{}

    public class ObtenerEspecialidadesQuery : Query<IEnumerable<Especialidad>>, IObtenerEspecialidadesQuery
    {
        public override IEnumerable<Especialidad> CoreExecute()
        {
            return SessionFactory.GetCurrentSession().QueryOver<Especialidad>().List();
        }
    }
}