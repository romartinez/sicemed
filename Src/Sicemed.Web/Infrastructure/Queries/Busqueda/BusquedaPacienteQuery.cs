using System.Collections.Generic;
using NHibernate.Criterion;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.Enumerations.Documentos;
using Sicemed.Web.Models.ViewModels;

namespace Sicemed.Web.Infrastructure.Queries.Busqueda
{
    public interface IBusquedaPacienteQuery : IQuery<IEnumerable<InfoViewModel>>
    {
        int? TipoDocumento { get; set; }
        long? NumeroDocumento { get; set; }
        string Nombre { get; set; }        
    }

    public class BusquedaPacienteQuery : Query<IEnumerable<InfoViewModel>>, IBusquedaPacienteQuery
    {
        public int? TipoDocumento { get; set; }
        public long? NumeroDocumento { get; set; }
        public string Nombre { get; set; }

        protected override IEnumerable<InfoViewModel> CoreExecute()
        {
            var session = SessionFactory.GetCurrentSession();

            var query = session.QueryOver<Models.Roles.Paciente>()
                .JoinQueryOver(x => x.Persona);

            if (!string.IsNullOrWhiteSpace(Nombre))
            {
                query.Where(
                    Restrictions.On<Persona>(x => x.Nombre).IsInsensitiveLike(Nombre, MatchMode.Anywhere)
                    || Restrictions.On<Persona>(x => x.SegundoNombre).IsInsensitiveLike(Nombre, MatchMode.Anywhere)
                    || Restrictions.On<Persona>(x => x.Apellido).IsInsensitiveLike(Nombre, MatchMode.Anywhere)
                );
            }

            if (TipoDocumento.HasValue)
            {
                var tipoDocumento = Enumeration.FromValue<TipoDocumento>(TipoDocumento.Value);
                query.Where(x => x.Documento.TipoDocumento == tipoDocumento);
            }

            if (NumeroDocumento.HasValue)
            {
                query.Where(x => x.Documento.Numero == NumeroDocumento.Value);
            }

            var pacientes = query.Future();

            return MappingEngine.Map<IEnumerable<InfoViewModel>>(pacientes);
        }
    }
}