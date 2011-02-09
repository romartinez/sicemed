namespace Sicemed.Model.Components {
    public abstract class Documento : ValueTypeBase {
        public abstract string Descripcion { get; }
        public abstract string DescripcionCorta { get; }
        public virtual long Numero { get; set; }
    }
}