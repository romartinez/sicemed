using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class EspecialidadProfesionalMap : EntityMapping<EspecialidadProfesional>
    {
         public EspecialidadProfesionalMap()
         {
             ManyToOne(x => x.Especialidad, map => map.NotNullable(true));
             ManyToOne(x => x.Profesional, map => map.NotNullable(true));
         }
    }
}