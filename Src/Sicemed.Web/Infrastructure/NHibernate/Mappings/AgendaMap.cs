using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class AgendaMap : EntityMapping<Agenda>
    {
        public AgendaMap()
        {
            Table("Agendas");
            Property(x => x.Dia);
            Property(x => x.DuracionTurno);
            Property(x => x.HorarioDesde);
            Property(x => x.HorarioHasta);

            ManyToOne(x => x.Consultorio, map => map.ForeignKey("FK_Agenda_Cosultorio"));
            ManyToOne(x => x.Profesional, map => map.ForeignKey("FK_Agenda_Profesional"));

            Set(x => x.EspecialidadesAtendidas, map =>
                                                {
                                                    map.Access(Accessor.NoSetter);
                                                    map.Lazy(CollectionLazy.NoLazy);
                                                    map.Key(k =>
                                                    {
                                                        k.ForeignKey("FK_AgendaEspecialidadesAtendida_Agenda");
                                                        k.Column("AgendaId");
                                                    });
                                                    map.Table("AgendaEspecialidadesAtendidas");
                                                }, rel => rel.ManyToMany());
        }
    }
}