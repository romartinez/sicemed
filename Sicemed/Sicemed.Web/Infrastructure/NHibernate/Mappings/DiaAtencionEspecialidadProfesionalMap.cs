using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class DiaAtencionEspecialidadProfesionalMap : EntityMapping<DiaAtencionEspecialidadProfesional>
    {
         public DiaAtencionEspecialidadProfesionalMap()
         {
             Property(x => x.DiaSemanaNombre);
             Property(x => x.DiaSemanaNumero);
             Property(x => x.DuracionTurno);
             Property(x => x.PoliticaHorariaMatutinaDesde);
             Property(x => x.PoliticaHorariaMatutinaHasta);
             Property(x => x.PoliticaHorariaVespertinaDesde);
             Property(x => x.PoliticaHorariaVespertinaHasta);

             ManyToOne(x => x.Consultorio);
             ManyToOne(x => x.EspecialidadProfesional);
         }
    }
}