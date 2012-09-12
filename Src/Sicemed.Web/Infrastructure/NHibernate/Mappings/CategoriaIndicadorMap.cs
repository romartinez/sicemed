using Sicemed.Web.Models.BI;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class CategoriaIndicadorMap : EntityMapping<CategoriaIndicador>
    {
        public CategoriaIndicadorMap()
        {
            Table("IndicadorCategorias");

            Property(x => x.Nombre, map => map.NotNullable(true));
        }
    }
}