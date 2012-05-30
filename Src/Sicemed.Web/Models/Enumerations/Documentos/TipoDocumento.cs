namespace Sicemed.Web.Models.Enumerations.Documentos
{
    public class TipoDocumento : Enumeration
    {
        public static readonly TipoDocumento Dni = new Dni();
        public static readonly TipoDocumento Cuit = new Cuit();

        public static readonly TipoDocumento[] TiposDeDocumentos = new[] {Dni, Cuit};

        public TipoDocumento() {}
        public TipoDocumento(long value, string displayName) : base(value, displayName) {}
    }
}