using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class FeriadoMap : EntityMapping<Feriado>
    {
        public FeriadoMap()
        {
            Property(x => x.FechaAjustada);
            Property(x => x.FechaOriginal, map =>
            {
                map.NotNullable(true);
                map.Index("IX_Feriado_FechaOriginal");
            });
            Property(x => x.Nombre, map => map.NotNullable(true));
        }
    }
}