using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ConsultorioMap : EntityMapping<Consultorio>
    {
         public ConsultorioMap()
         {
             Property(x => x.Descripcion);
             Property(x => x.Nombre, map => map.NotNullable(true));
         }
    }
}