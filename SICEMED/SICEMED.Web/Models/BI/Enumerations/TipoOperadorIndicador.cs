using Sicemed.Web.Models.Enumerations;

namespace Sicemed.Web.Models.BI.Enumerations
{
    public class TipoOperadorIndicador : Enumeration
    {
        public static readonly TipoOperadorIndicador Mayor = new TipoOperadorIndicador(1, "Mayor");
        public static readonly TipoOperadorIndicador Menor = new TipoOperadorIndicador(-1, "Menor");

        protected TipoOperadorIndicador(long value, string displayName) : base(value, displayName) { }
    }
}