using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class UsuarioMap : EntityMapping<Usuario>
    {
        public UsuarioMap()
        {
            Property(x => x.Nombre);
            Property(x => x.SegundoNombre);
            Property(x => x.Apellido);
            Property(x => x.FechaNacimiento);
            Property(x => x.EstaHabilitadoTurnosWeb);
            Property(x => x.InasistenciasContinuas);
            Property(x => x.NumeroAfiliado);

            Component(x => x.Membership);
            Component(x => x.Documento);
            Component(x => x.Domicilio);
            Component(x => x.Telefono);

            Set(x => x.Roles, map =>
                              {
                                  map.Key(x => x.PropertyRef(o => o.Id));
                                  map.Access(Accessor.Field);
                              });
        }
    }
}