using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models.BI;
using Sicemed.Web.Models.BI.Enumerations;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class IndicadorMap : EntityMapping<Indicador>
    {
        public IndicadorMap()
        {
            Table("Indicadores");

            Property(x => x.Nombre, map => map.NotNullable(true));
            Property(x => x.Descripcion);
            Property(x => x.DenominadorSql);
            Property(x => x.NumeradorSql);
            Property(x => x.Habilitado);

            Property(x => x.TipoOperador, map => map.Type<EnumerationType<TipoOperadorIndicador>>());

            ManyToOne(x => x.Categoria, m =>
                {
                    m.ForeignKey("FK_IndicadorCategorias_Indicadores");
                    m.Column("CategoriaId");
                });

            Set(x => x.Objetivos, map =>
            {
                map.Inverse(true);
                map.Access(Accessor.NoSetter);
                map.Cascade(Cascade.All | Cascade.DeleteOrphans);
                map.Key(k =>
                    {
                        k.Column("IndicadorId"); 
                        k.ForeignKey("FK_IndicadorObjetivos_Indicador");
                    });
            }, rel => rel.OneToMany());

        }
    }
}