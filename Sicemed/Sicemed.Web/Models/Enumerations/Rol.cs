namespace Sicemed.Web.Models.Enumerations
{
    public class Rol : Enumeration
    {
        public static readonly Rol Secretaria = new Rol(1, "Secretaria");
        public static readonly Rol Profesional = new Rol(2, "Profesional");
        public static readonly Rol Administrador = new Rol(3, "Administrador");
        
        public Rol(){}
        public Rol(long value, string displayName) : base(value, displayName) { }
    }
}