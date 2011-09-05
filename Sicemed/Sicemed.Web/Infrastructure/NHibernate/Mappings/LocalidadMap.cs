using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class LocalidadMap : EntityMapping<Localidad>
    {
         public LocalidadMap()
         {
             Property(x => x.Caracteristicas);
             Property(x => x.CodigoPostal);
             Property(x => x.Nombre);

             ManyToOne(x => x.Provincia);
         }
    }
}