using System.Collections.Generic;

namespace Sicemed.Model {
    public class Localidad : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string CodigoPostal { get; set; }

        public virtual string Caracteristicas { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Provincia Provincia { get; set; }

        public virtual ISet<ObraSocial> ObrasSociales { get; set; }

        public virtual ISet<Paciente> Pacientes { get; set; }

        public virtual ISet<Profesional> Profesionales { get; set; }

        public virtual ISet<Secretaria> Secretarias { get; set; }

        #endregion
    }
}