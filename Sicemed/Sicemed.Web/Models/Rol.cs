using Sicemed.Web.Models.Components;

namespace Sicemed.Web.Models
{
    public class Rol : Enumeration
    {
        public static readonly Rol Secretaria = new Rol(1, "Secretaria");
        public static Rol Profesional = new Rol(2, "Profesional");
        public static Rol Administrador = new Rol(3, "Administrador");
        
        public Rol(){}
        public Rol(long value, string displayName) : base(value, displayName) { }
    }
}