using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ObraSocialMap : EntityMapping<ObraSocial>
    {
         public ObraSocialMap()
         {
             Property(x => x.RazonSocial, map => map.NotNullable(true));

             Component(x => x.Documento);
             Component(x => x.Direccion);
             Component(x => x.Telefono);

             Set(x => x.Planes, map => map.Inverse(true), rel => rel.OneToMany());
         }
    }
}