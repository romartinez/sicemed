using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels.ObtenerTurno;

namespace Sicemed.Web.Infrastructure.Queries.ObtenerTurno
{
    public interface IObtenerProfesionalPorEspecialidadONombreQuery : IQuery<IEnumerable<BusquedaProfesionalViewModel>>
    {
        long? EspecialidadId { get; set; }
        string Profesional { get; set; }
    }

    public class ObtenerProfesionalPorEspecialidadONombreQuery : Query<IEnumerable<BusquedaProfesionalViewModel>>, IObtenerProfesionalPorEspecialidadONombreQuery
    {
        public virtual long? EspecialidadId { get; set; }
        public virtual string Profesional { get; set; }

        protected override IEnumerable<BusquedaProfesionalViewModel> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Persona>();
            if (!string.IsNullOrWhiteSpace(Profesional))
            {
                query = query.Where(
                    Restrictions.On<Persona>(p => p.Nombre).IsLike(Profesional, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.SegundoNombre).IsLike(Profesional, MatchMode.Start)
                    || Restrictions.On<Persona>(p => p.Apellido).IsLike(Profesional, MatchMode.Start)
                    );
            }

            var queryRoles = query.JoinQueryOver<Rol>(p => p.Roles)
                    .Where(r => r.GetType() == typeof(Models.Roles.Profesional));

            IEnumerable<Persona> results;

            if (EspecialidadId.HasValue)
            {
                results = queryRoles.JoinQueryOver<Especialidad>(r => ((Models.Roles.Profesional)r).Especialidades)
                    .Where(e => e.Id == EspecialidadId)
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();
            }
            else
            {
                results = queryRoles.TransformUsing(Transformers.DistinctRootEntity).Future();
            }

            var profesionales = results.Select(BusquedaProfesionalViewModel.Create);
            return profesionales;
        }
    }
}