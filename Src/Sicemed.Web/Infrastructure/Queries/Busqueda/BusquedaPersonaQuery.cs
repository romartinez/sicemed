using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Roles;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Infrastructure.Queries.Busqueda
{
    public interface IBusquedaPersonaQuery : IQuery<IEnumerable<PersonaViewModel>>
    {
        Type Rol { get; set; }
        string Filtro { get; set; }
    }

    public class BusquedaPersonaQuery : Query<IEnumerable<PersonaViewModel>>, IBusquedaPersonaQuery
    {
        public Type Rol { get; set; }
        public string Filtro { get; set; }

        protected override IEnumerable<PersonaViewModel> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Persona>();

            if (!string.IsNullOrWhiteSpace(Filtro))
            {
                query.Where(
                    Restrictions.On<Persona>(x => x.Nombre).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                    || Restrictions.On<Persona>(x => x.SegundoNombre).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                    || Restrictions.On<Persona>(x => x.Apellido).IsInsensitiveLike(Filtro, MatchMode.Anywhere)
                    || Restrictions.Like(Projections.Cast(NHibernateUtil.String, Projections.Property<Persona>(x => x.Documento.Numero)), Filtro, MatchMode.Anywhere)
                    );
            }

            if (Rol != null)
            {
                query.JoinQueryOver<Rol>(x => x.Roles).Where(x => x.GetType() == Rol);
            }

            var personas = query.Future();

            return MappingEngine.Map<IEnumerable<PersonaViewModel>>(personas);
        }
    }
}