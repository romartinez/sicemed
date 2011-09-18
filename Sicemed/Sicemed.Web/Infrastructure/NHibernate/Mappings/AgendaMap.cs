using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class AgendaMap : EntityMapping<Agenda>
    {
        public AgendaMap()
        {
            Property(x => x.Dia);
            Property(x => x.DuracionTurno);
            Property(x => x.HorarioDesde);
            Property(x => x.HorarioHasta);

            ManyToOne(x => x.Consultorio, map => map.ForeignKey("FK_Agenda_Cosultorio"));
            ManyToOne(x => x.Profesional, map => map.ForeignKey("FK_Agenda_Profesional"));

            Set(x => x.EspecialidadesAtendidas, map =>
                                                {
                                                    map.Access(Accessor.NoSetter);
                                                    map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                                                }, rel => rel.ManyToMany());
        }
    }
}