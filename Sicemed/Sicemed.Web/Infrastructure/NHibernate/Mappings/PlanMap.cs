using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class PlanMap : EntityMapping<Plan>
    {

        public PlanMap()
        {
            Property(x => x.Descripcion, map => map.NotNullable(true));
            Property(x => x.Nombre, map => map.NotNullable(true));

            ManyToOne(x => x.ObraSocial, map => map.NotNullable(true));
        }
         
    }
}