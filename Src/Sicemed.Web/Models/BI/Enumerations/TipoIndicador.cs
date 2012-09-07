using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.BI.Enumerations
{
    public class TipoIndicador : Enumeration
    {
        public static readonly TipoIndicador Gerencial = new TipoIndicador(1, "Gerencial");
        public static readonly TipoIndicador Operativo = new TipoIndicador(2, "Operativo");

        protected TipoIndicador(long value, string displayName) : base(value, displayName) { }
    }
}