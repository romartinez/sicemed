using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ProvinciaMap : EntityMapping<Provincia>
    {
         public ProvinciaMap()
         {
             Property(x => x.Nombre);

             Set(x => x.Localidades, map => map.Inverse(true), rel=>rel.OneToMany());
         }
    }
}