using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class EspecialidadMap : EntityMapping<Especialidad>
    {
        public EspecialidadMap()
        {
            Property(x => x.Descripcion);
            Property(x => x.Nombre, map =>
            {
                map.NotNullable(true);
                map.Unique(true);
            });
        }
    }
}