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

            Component(x => x.Membership, map=> map.Access(Accessor.NoSetter));
            Component(x => x.Documento);
            Component(x => x.Domicilio);
            Component(x => x.Telefono);

            Set(x => x.Roles, map => { map.Access(Accessor.NoSetter);
                                         map.Cascade(Cascade.All);
            }, rel => rel.Component(map => { }));
            
            Set(x => x.Turnos, map => { map.Inverse(true); }, rel => rel.OneToMany());
        }
    }
}