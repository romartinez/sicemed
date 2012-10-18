using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class ClinicaMap : EntityMapping<Clinica>
    {
        public ClinicaMap()
        {
            Table("Clinicas");
            Property(x => x.RazonSocial, map =>
            {
                map.Unique(true);
                map.NotNullable(true);
            });
            Property(x => x.HorarioMatutinoDesde, map => map.NotNullable(true));
            Property(x => x.HorarioMatutinoHasta, map => map.NotNullable(true));

            Property(x => x.DuracionTurnoPorDefecto);

            Property(x => x.HorarioVespertinoDesde);
            Property(x => x.HorarioVespertinoHasta);

            Property(x => x.GoogleMapsKey);

            Property(x => x.NumeroInasistenciasConsecutivasGeneranBloqueo, map => map.NotNullable(true));
            Property(x => x.Email, map => map.NotNullable(true));

            Component(x => x.Documento);
            Component(x => x.Domicilio);

            Set(x => x.Telefonos, map =>
                                      {
                                          map.Access(Accessor.NoSetter);
                                          map.Lazy(CollectionLazy.NoLazy);
                                          map.Key(k =>
                                                      {
                                                          k.ForeignKey("FK_Clinica_ClinicaTelefono");
                                                          k.Column("ClinicaId");
                                                      });
                                          map.Table("ClinicaTelefonos");
                                      }, rel => rel.Component(m => { }));

            Set(x => x.DiasHabilitados, map =>
                                      {
                                          map.Access(Accessor.NoSetter);
                                          map.Lazy(CollectionLazy.NoLazy);
                                          map.Key(k =>
                                          {
                                              k.ForeignKey("FK_Clinica_ClinicaDiasHabilitados");
                                              k.Column("ClinicaId");
                                          });
                                          map.Table("ClinicaDiasHabilitados");
                                      });
        }
    }
}