namespace Sicemed.Web.Models.Roles
{
    public class Administrador : Rol
    {
        protected Administrador() {}

        public override string DisplayName
        {
            get { return ADMINISTRADOR; }
        }

        public static Rol Create()
        {
            return new Administrador();
        }
    }
}