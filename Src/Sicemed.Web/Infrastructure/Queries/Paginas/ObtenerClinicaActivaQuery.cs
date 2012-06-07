using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerClinicaActivaQuery : IQuery<Clinica> { }

    public class ObtenerClinicaActivaQuery : Query<Clinica>, IObtenerClinicaActivaQuery
    {
        protected override Clinica CoreExecute()
        {
			//En multitenant leer el ID desde el config.
            return SessionFactory.GetCurrentSession().QueryOver<Clinica>().OrderBy(x => x.Id).Desc.SingleOrDefault();
        }
    }
}