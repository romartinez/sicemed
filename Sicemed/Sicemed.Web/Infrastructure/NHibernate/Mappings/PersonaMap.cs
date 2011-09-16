using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class PersonaMap : EntityMapping<Persona>
    {
        public PersonaMap()
        {
            Property(x => x.Nombre, map => map.NotNullable(true));
            Property(x => x.Apellido, map => map.NotNullable(true));
            Property(x => x.SegundoNombre);
            Property(x => x.FechaNacimiento);            

            Component(x => x.Membership, map => map.Access(Accessor.NoSetter));
            Component(x => x.Documento);
            Component(x => x.Domicilio);
            Component(x => x.Telefono);

            Set(x => x.Roles, map =>
            {
                map.Inverse(true);
                map.Access(Accessor.NoSetter);
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
            }, rel => rel.OneToMany() );
        }
    }
}