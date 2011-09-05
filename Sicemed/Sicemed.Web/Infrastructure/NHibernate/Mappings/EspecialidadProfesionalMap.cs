using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class EspecialidadProfesionalMap : EntityMapping<EspecialidadProfesional>
    {
         public EspecialidadProfesionalMap()
         {
             ManyToOne(x => x.Especialidad);
             ManyToOne(x => x.Profesional);
         }
    }
}