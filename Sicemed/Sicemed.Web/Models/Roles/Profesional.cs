namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        public override string DisplayName
        {
            get { return PROFESIONAL; }
        }

        public virtual string Matricula { get; set; }

        protected Profesional() { }

        public static Rol Create(string matricula)
        {
            return new Profesional() { Matricula = matricula };
        }

    }
}