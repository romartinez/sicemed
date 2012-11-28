using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class TurnoMap : EntityMapping<Turno>
    {
        public TurnoMap()
        {
            Table("Turnos");

            Property(x => x.FechaTurno, map => map.NotNullable(true));
            Property(x => x.DuracionTurno, map => map.NotNullable(true));
            Property(x => x.IpPaciente);
            Property(x => x.EsTelefonico, map => map.NotNullable(true));
            Property(x => x.EsSobreTurno, map => map.NotNullable(true));
            Property(x => x.EsObtenidoWeb, map => map.NotNullable(true));
            Property(x => x.Nota, map => map.Length(SqlClientDriver.MaxSizeForLengthLimitedString + 1));  //map => { map.Column(c => {c.SqlType("varchar(8000)");}); });
            Property(x => x.MotivoCancelacion, map => map.Length(SqlClientDriver.MaxSizeForLengthLimitedString + 1));//{ map.Column(c => { c.SqlType("varchar(8000)"); }); });
            Property(x => x.Estado, map => map.NotNullable(true));
            Property(x => x.FechaEstado, map => map.NotNullable(true));
            Property(x => x.NumeroAfiliado);
            Property(x => x.Coseguro);
            ManyToOne(x => x.Paciente, map =>
                                       {
                                           map.NotNullable(true);
                                           map.ForeignKey("FK_Turno_PersonaRol_Paciente");
                                       });
            ManyToOne(x => x.Profesional, map =>
                                          {
                                              map.NotNullable(true);
                                              map.ForeignKey("FK_Turno_PersonaRol_Profesional");
                                          });
            ManyToOne(x => x.Especialidad, map =>
                                           {
                                               map.NotNullable(true);
                                               map.ForeignKey("FK_Turno_Especialidad");
                                           });
            ManyToOne(x => x.Consultorio, map => map.ForeignKey("FK_Turno_Consultorio"));
            ManyToOne(x => x.Plan, map => map.ForeignKey("FK_Turno_Plan"));
            List(x => x.CambiosDeEstado, collectionMap =>
                {
                    collectionMap.Table("TurnosCambioEstados");
                    collectionMap.Lazy(CollectionLazy.NoLazy);
                    collectionMap.Fetch(CollectionFetchMode.Join);
                    collectionMap.Key(m =>
                        {
                            m.Column("TurnoId"); 
                            m.ForeignKey("FK_TurnosCambioEstados_Turno");
                        });
                }, rel => rel.Component(m =>
                    {
                        m.Property(x => x.Estado, x => x.Column("Estado"));
                        m.Property(x => x.Evento, x => x.Column("Evento"));
                        m.Property(x => x.Fecha, x => x.Column("Fecha"));
                        m.ManyToOne(x => x.Responsable, x => x.Column("Responsable"));
                    }));
        }
    }
}