using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ObraSocialMap : EntityMapping<ObraSocial>
    {
         public ObraSocialMap()
         {
             Property(x => x.RazonSocial, map => map.NotNullable(true));

             Component(x => x.Documento);
             Component(x => x.Domicilio);
             Component(x => x.Telefono);

             Set(x => x.Planes, map =>
                                {
                                    map.Inverse(true);
                                    map.Access(Accessor.NoSetter);
                                }, rel => rel.OneToMany());
         }
    }
}