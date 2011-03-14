namespace Sicemed.Web.Models {
    public class Localidad : EntityBase {
        #region Primitive Properties

        public virtual string Nombre { get; set; }

        public virtual string CodigoPostal { get; set; }

        public virtual string Caracteristicas { get; set; }

        #endregion

        #region Navigation Properties

        public virtual Provincia Provincia { get; set; }

        #endregion
    }
}