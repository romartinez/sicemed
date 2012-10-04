using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using Sicemed.Web.Models.Components;
using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings.Components
{
    public class CambioEstadoTurnoMap : ComponentMapping<CambioEstadoTurno>
    {
        public CambioEstadoTurnoMap()
        {
            Property(x => x.Estado, m => { m.NotNullable(true); m.Type<EnumStringType<EstadoTurno>>(); });
            Property(x => x.Evento, m => { m.NotNullable(true); m.Type<EnumStringType<EventoTurno>>(); });
            Property(x => x.Fecha, x => x.NotNullable(true));
            ManyToOne(x => x.Responsable, m => m.ForeignKey("FK_TurnosCambioEstados_Personal"));
        }
    }
}