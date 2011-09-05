using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class FeriadoMap : EntityMapping<Feriado>
    {
         public FeriadoMap()
         {
             Property(x => x.FechaAjustada);
             Property(x => x.FechaOriginal);
             Property(x => x.Nombre);

             ManyToOne(x => x.Calendario);
         }
    }
}