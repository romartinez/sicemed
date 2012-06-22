using System;
using System.Collections.Generic;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.Queries.Jobs
{
    public interface IObtenerTurnosAMarcarComoAusentadosQuery : IQuery<IEnumerable<Turno>> { }

    public class ObtenerTurnosAMarcarComoAusentadosQuery : Query<IEnumerable<Turno>>, IObtenerTurnosAMarcarComoAusentadosQuery
    {
        protected override IEnumerable<Turno> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Turno>()
                .Where(t => t.Estado == Turno.EstadoTurno.Otorgado
                    && t.FechaTurno <= DateTime.Now.AddDays(-1))
                .Future();

            return query;
        }
    }
}