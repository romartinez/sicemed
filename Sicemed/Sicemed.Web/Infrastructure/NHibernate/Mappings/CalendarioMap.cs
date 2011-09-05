using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class CalendarioMap : EntityMapping<Calendario>
    {
         public CalendarioMap()
         {
             Property(x => x.Anio);
             Property(x => x.Nombre);

             Set(x => x.Feriados, map => { map.Access(Accessor.NoSetter);
                                             map.Inverse(true);
             }, rel=> rel.OneToMany());
         }
    }
}