using System;

namespace Sicemed.Web.Models
{
    public class DiaAtencionEspecialidadProfesional : Entity
    {
        #region Primitive Properties

        public virtual int DiaSemanaNumero { get; set; }

        public virtual string DiaSemanaNombre { get; set; }

        public virtual DateTime DuracionTurno { get; set; }

        public virtual DateTime? PoliticaHorariaVespertinaHasta { get; set; }

        public virtual DateTime? PoliticaHorariaVespertinaDesde { get; set; }

        public virtual DateTime? PoliticaHorariaMatutinaHasta { get; set; }

        public virtual DateTime? PoliticaHorariaMatutinaDesde { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Consultorio Consultorio { get; set; }

        public virtual EspecialidadProfesional EspecialidadProfesional { get; set; }

        #endregion
    }
}