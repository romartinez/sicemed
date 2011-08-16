


using System.Collections.Generic;

namespace Sicemed.Web.Models
{
    public class Consultorio : Entity
    {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string Descripcion { get; set; }

        #endregion

        #region Navigation Properties

        public virtual ISet<DiaAtencionEspecialidadProfesional> DiasAtencionesEspecialidadesProfesionales { get; set; }

        #endregion
    }
}