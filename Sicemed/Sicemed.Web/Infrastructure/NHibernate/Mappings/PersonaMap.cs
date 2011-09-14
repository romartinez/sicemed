using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Components.Roles;

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
            Property(x => x.EstaHabilitadoTurnosWeb);
            Property(x => x.InasistenciasContinuas, map => map.NotNullable(true));
            Property(x => x.NumeroAfiliado);

            Component(x => x.Membership, map => map.Access(Accessor.NoSetter));
            Component(x => x.Documento);
            Component(x => x.Domicilio);
            Component(x => x.Telefono);

            Set(x => x.Roles, map =>
            {
                map.Access(Accessor.NoSetter);
                map.Cascade(Cascade.All);
            }, rel => rel.Component(map=>{}));

            //Set(x => x.Turnos, map =>
            //{
            //    map.Inverse(true);
            //    map.Access(Accessor.NoSetter);
            //}, rel => rel.OneToMany());
        }
    }
}