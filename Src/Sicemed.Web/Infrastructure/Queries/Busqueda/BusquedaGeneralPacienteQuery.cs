using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Sicemed.Web.Models;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Infrastructure.Queries.Busqueda
{
    public interface IBusquedaGeneralPacienteQuery : IQuery<IEnumerable<PersonaViewModel>>
    {
        string Filtro { get; set; }
    }

    public class BusquedaGeneralPacienteQuery : Query<IEnumerable<PersonaViewModel>>, IBusquedaGeneralPacienteQuery
    {
        public string Filtro { get; set; }

        protected override IEnumerable<PersonaViewModel> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Models.Roles.Paciente>();

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                query.JoinQueryOver(p => p.Persona).Where(
                    Restrictions.On<Persona>(x => x.Nombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                    || Restrictions.On<Persona>(x => x.SegundoNombre).IsInsensitiveLike(Filtro, MatchMode.Start)
                    || Restrictions.On<Persona>(x => x.Apellido).IsInsensitiveLike(Filtro, MatchMode.Start)
                    || Restrictions.Like(Projections.Cast(NHibernateUtil.String, Projections.Property<Persona>(x => x.Documento.Numero)), Filtro, MatchMode.Start)
                    );
            }

            var personas = query.Future();

            return MappingEngine.Map<IEnumerable<PersonaViewModel>>(personas);
        }
    }
}