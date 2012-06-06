using NHibernate;
using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class PaginaMap : EntityMapping<Pagina>
    {
        public PaginaMap()
        {
            Property(x => x.Contenido, map =>
                                       {
                                           map.NotNullable(true);
                                           map.Type(NHibernateUtil.StringClob);
                                       });
            Property(x => x.Nombre, map =>
                                    {
                                        map.NotNullable(true);
                                        map.Unique(true);
                                    });
            Property(x => x.Url, map =>
                                    {
                                        map.NotNullable(true);
                                        map.Unique(true);
                                    });
            Property(x => x.Orden, map => map.NotNullable(true));

            ManyToOne(x => x.Padre, map => map.ForeignKey("FK_Paginas_Padre"));

            Set(
                x => x.Hijos,
                map =>
                {
                    map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                    map.Access(Accessor.NoSetter);
                    map.Inverse(true);
                    map.Lazy(CollectionLazy.NoLazy);
                },
                rel => rel.OneToMany());
        }
    }
}