using Sicemed.Web.Models.BI;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ObjetivoIndicadorMap : EntityMapping<ObjetivoIndicador>
    {
        public ObjetivoIndicadorMap()
        {
            Table("IndicadorObjetivos");

            Property(x => x.Anio, map => map.NotNullable(true));
            Property(x => x.Mes, map => map.NotNullable(true));
            Property(x => x.ValorMaximo, map => map.NotNullable(true));
            Property(x => x.ValorMinimo, map => map.NotNullable(true));

            Property(x => x.Valor);            
            Property(x => x.FechaLectura);            

            ManyToOne(x => x.Indicador, map =>
            {
                map.NotNullable(true);
                map.ForeignKey("FK_IndicadorObjetivos_Indicador");
            });            
        }
    }
}