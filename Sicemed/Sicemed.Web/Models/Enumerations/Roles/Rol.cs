namespace Sicemed.Web.Models.Enumerations.Roles
{
    public class Rol : Enumeration
    {
        public static readonly Rol Secretaria = new Secretaria();
        public static readonly Rol Profesional = new Profesional();
        public static readonly Rol Administrador = new Administrador();

        public static readonly Rol[] Roles = new[] { Secretaria, Profesional, Administrador };

        public Rol() { }
        public Rol(long value, string displayName) : base(value, displayName) { }
    }
}