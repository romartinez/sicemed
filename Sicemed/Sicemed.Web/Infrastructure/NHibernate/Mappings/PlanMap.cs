using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class PlanMap : EntityMapping<Plan>
    {

        public PlanMap()
        {
            Property(x => x.Descripcion);
            Property(x => x.Nombre);

            ManyToOne(x=>x.ObraSocial);
        }
         
    }
}