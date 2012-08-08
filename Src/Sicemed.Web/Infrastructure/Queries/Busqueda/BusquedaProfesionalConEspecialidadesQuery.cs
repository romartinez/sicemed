using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using Sicemed.Web.Infrastructure.Exceptions;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels.Profesional;

namespace Sicemed.Web.Infrastructure.Queries.Busqueda
{
    public interface IBusquedaProfesionalConEspecialidadesQuery : IQuery<IEnumerable<ProfesionalConEspecialidadesViewModel>>
    {
        string Filtro { get; set; }
    }

    public class BusquedaProfesionalConEspecialidadesQuery : Query<IEnumerable<ProfesionalConEspecialidadesViewModel>>, IBusquedaProfesionalConEspecialidadesQuery
    {
        public string Filtro { get; set; }

        protected override IEnumerable<ProfesionalConEspecialidadesViewModel> CoreExecute()
        {
            if(string.IsNullOrWhiteSpace(Filtro) || Filtro.Length < 3) throw new ValidationErrorException("Debe ingresar al menos 3 caracteres para efectuar la búsqueda.");

            var session = SessionFactory.GetCurrentSession();
            Especialidad especialidad = null;
            var query = session.QueryOver<Models.Roles.Profesional>();
            query.JoinQueryOver(p => p.Especialidades,() => especialidad);
            query.JoinQueryOver(p => p.Persona).Where(
                Restrictions.On<Persona>(x => x.Nombre).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                || Restrictions.On<Persona>(x => x.SegundoNombre).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                || Restrictions.On<Persona>(x => x.Apellido).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                || Restrictions.Like(Projections.Cast(NHibernateUtil.String, Projections.Property<Persona>(x => x.Documento.Numero)), Filtro, MatchMode.Anywhere)
                || Restrictions.On<Especialidad>(x => especialidad.Nombre).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
            ).TransformUsing(new DistinctRootEntityResultTransformer());

            var personas = query.Future();

            return MappingEngine.Map<IEnumerable<ProfesionalConEspecialidadesViewModel>>(personas);
        }
    }
}