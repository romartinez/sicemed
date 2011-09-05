using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class LocalidadMap : EntityMapping<Localidad>
    {
         public LocalidadMap()
         {
             Property(x => x.Caracteristicas, map => map.NotNullable(true));
             Property(x => x.CodigoPostal);
             Property(x => x.Nombre, map => map.NotNullable(true));

             ManyToOne(x => x.Provincia, map => map.NotNullable(true));
         }
    }
}