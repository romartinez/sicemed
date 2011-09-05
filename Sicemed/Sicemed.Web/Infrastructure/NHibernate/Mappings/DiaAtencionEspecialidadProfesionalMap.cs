using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.NHibernate.Mappings
{
    public class DiaAtencionEspecialidadProfesionalMap : EntityMapping<DiaAtencionEspecialidadProfesional>
    {
         public DiaAtencionEspecialidadProfesionalMap()
         {
             Property(x => x.DiaSemanaNombre, map => map.NotNullable(true));
             Property(x => x.DiaSemanaNumero, map => map.NotNullable(true));
             Property(x => x.DuracionTurno, map => map.NotNullable(true));
             Property(x => x.PoliticaHorariaMatutinaDesde, map => map.NotNullable(true));
             Property(x => x.PoliticaHorariaMatutinaHasta, map => map.NotNullable(true));
             Property(x => x.PoliticaHorariaVespertinaDesde, map => map.NotNullable(true));
             Property(x => x.PoliticaHorariaVespertinaHasta, map => map.NotNullable(true));

             ManyToOne(x => x.Consultorio);
             ManyToOne(x => x.EspecialidadProfesional, map => map.NotNullable(true));
         }
    }
}