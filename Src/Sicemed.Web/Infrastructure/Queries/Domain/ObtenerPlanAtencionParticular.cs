using System;
using System.Collections.Generic;
using System.Linq;
using Sicemed.Web.Infrastructure.Helpers;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.ViewModels.Profesional;

namespace Sicemed.Web.Infrastructure.Queries.Domain
{
    public interface IObtenerPlanAtencionParticular : IQuery<IEnumerable<Plan>>
    {
        long PlanId { get; set; }
    }

    public class ObtenerPlanAtencionParticular : Query<IEnumerable<Plan>>, IObtenerPlanAtencionParticular
    {
        public virtual long PlanId { get; set; }

        protected override IEnumerable<Plan> CoreExecute()
        {
            var descripcion = "Consulta Particular";
            var session = SessionFactory.GetCurrentSession();

            var plan = session.QueryOver<Plan>()
                .Fetch(t => t.Id).Eager
                .Fetch(t => t.Nombre).Eager
                .Fetch(t => t.Descripcion).Eager
                .Fetch(t => t.Coseguro).Eager
                .Fetch(t => t.ObraSocial).Eager
                .Where(t => t.Nombre == descripcion)
                .List();

            return plan;
        }
    }
}