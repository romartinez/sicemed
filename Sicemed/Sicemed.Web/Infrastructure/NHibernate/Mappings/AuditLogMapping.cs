using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class AuditLogMapping : ClassMapping<AuditLog>
    {
        public AuditLogMapping()
        {            
            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property(x => x.Accion);
            Property(x => x.Entidad);
            Property(x => x.EntidadAntes, m => m.Type(NHibernateUtil.StringClob));
            Property(x => x.EntidadDespues, m => m.Type(NHibernateUtil.StringClob));
            Property(x => x.Fecha);
            Property(x => x.Usuario);
        }
    }
}