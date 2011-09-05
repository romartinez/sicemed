using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class PaginaMap : EntityMapping<Pagina>
    {
         public PaginaMap()
         {
             Property(x => x.Contenido);
             Property(x => x.Nombre);

             ManyToOne(x => x.Padre);

             Set(x => x.Hijos, map => { map.Access(Accessor.NoSetter);
                                          map.Inverse(true);
             }, rel => rel.OneToMany());
         }
    }
}