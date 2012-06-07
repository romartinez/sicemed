using System;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models.ViewModels.Secretaria;

namespace Sicemed.Web.Infrastructure.Queries.Secretaria
{
    public interface IObtenerTurnosPorFechaQuery : IQuery<TurnosDelDiaViewModel>
    {
        DateTime? Fecha { get; set; }
    }

    public class ObtenerTurnosPorFechaQuery : Query<TurnosDelDiaViewModel>, IObtenerTurnosPorFechaQuery
    {
        public virtual DateTime? Fecha { get; set; }

        protected override TurnosDelDiaViewModel CoreExecute()
        {
            var date = Fecha ?? DateTime.Now;
            date = date.ToMidnigth();

            return null;
        }
    }
}