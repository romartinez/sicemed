using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class AusenciaMap : EntityMapping<Ausencia>
    {
        public AusenciaMap()
        {
            Table("Ausencias");

            Property(x => x.Fecha, map => map.NotNullable(true));

            Property(x => x.Desde);
            Property(x => x.Hasta);

            ManyToOne(x=>x.Profesional, map =>
                                        {
                                            map.NotNullable(true);
                                            map.ForeignKey("FK_Ausencias_Profesional");
                                        });
        }
    }
}