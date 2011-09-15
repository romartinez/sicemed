namespace Sicemed.Web.Models.Roles
{
    public class Administrador : Rol
    {
        public override string DisplayName
        {
            get { return ADMINISTRADOR; }
        }

        protected Administrador() { }

        public static Rol Create()
        {
            return new Administrador();
        }
    }
}