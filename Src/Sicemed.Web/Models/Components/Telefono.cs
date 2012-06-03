namespace Sicemed.Web.Models.Components
{
    public class Telefono : ComponentBase
    {
        private string _prefijo = string.Empty;
        public virtual string Prefijo
        {
            get { return _prefijo; }
            set { _prefijo = value ?? string.Empty; }
        }

        private string _numero = string.Empty;
        public virtual string Numero
        {
            get { return _numero; }
            set { _numero = value ?? string.Empty; }
        }

        public bool Equals(Telefono other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Numero, Numero) && Equals(other.Prefijo, Prefijo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Numero != null ? Numero.GetHashCode() : 0) * 397) ^ (Prefijo != null ? Prefijo.GetHashCode() : 0);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Telefono)) return false;
            return Equals((Telefono)obj);
        }
    }
}