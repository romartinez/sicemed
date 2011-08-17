namespace Sicemed.Web.Models
{
    public class Rol : Entity
    {
        public static Rol Secretaria = new Rol() {Name = "Secretaria"};
        public static Rol Profesional = new Rol() { Name = "Profesional" };
        public static Rol Administrador = new Rol() { Name = "Administrador" };

        public virtual string Name { get; set; }         
    }
}