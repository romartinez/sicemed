using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class FeriadoMap : EntityMapping<Feriado>
    {
        public FeriadoMap()
        {
            Property(x => x.Fecha, map =>
                                   {
                                       map.NotNullable(true);
                                       map.Index("IX_Feriado_Fecha");
                                       map.Unique(true);
                                   });
            Property(x => x.Nombre, map => map.NotNullable(true));
        }
    }
}