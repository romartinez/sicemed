using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class DomicilioMap : ComponentMapping<Domicilio>
    {
        public DomicilioMap()
        {
            Property(x => x.Direccion);
            Property(x => x.Latitud);
            Property(x => x.Longitud);

            ManyToOne(x => x.Localidad);
        }
    }
}