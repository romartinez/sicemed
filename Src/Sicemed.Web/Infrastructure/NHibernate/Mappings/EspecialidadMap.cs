using NHibernate.Mapping.ByCode;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class EspecialidadMap : EntityMapping<Especialidad>
    {
        public EspecialidadMap()
        {
            Table("Especialidades");
            Property(x => x.Descripcion);
            Property(x => x.Nombre, map =>
            {
                map.NotNullable(true);
                map.Unique(true);
            });

            Set(x => x.Profesionales, map =>
                                          {
                                              map.Access(Accessor.NoSetter);
                                              map.Cascade(Cascade.None);
                                              map.Inverse(false);
                                              map.Key(k =>
                                              {
                                                  k.ForeignKey("FK_ProfesionalEspecialidad_Especialidad");
                                                  k.Column("EspecialidadId");
                                              });
                                              map.Table("ProfesionalEspecialidades");
                                          }, rel => rel.ManyToMany());
        }
    }
}