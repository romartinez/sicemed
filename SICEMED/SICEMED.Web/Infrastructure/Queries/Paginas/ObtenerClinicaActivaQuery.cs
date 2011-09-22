using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Paginas
{
    public interface IObtenerClinicaActivaQuery : IQuery<Clinica> { }

    public class ObtenerClinicaActivaQuery : Query<Clinica>, IObtenerClinicaActivaQuery
    {
        public override IEnumerable<Clinica> CoreExecute()
        {
            return new[]
                       {
                           SessionFactory.GetCurrentSession().QueryOver<Clinica>().OrderBy(x => x.Id).Desc.SingleOrDefault()
                       };
        }
    }
}