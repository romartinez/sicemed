using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class DomicilioMap : ComponentMapping<Domicilio>
    {
         public DomicilioMap ()
         {
             Property(x => x.Direccion);

             ManyToOne(x => x.Localidad, map=> map.ForeignKey("FK_Domicilios_Localidad"));
         }
    }
}