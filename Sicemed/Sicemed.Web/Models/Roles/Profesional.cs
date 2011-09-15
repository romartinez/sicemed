namespace Sicemed.Web.Models.Roles
{
    public class Profesional : Rol
    {
        public override string DisplayName
        {
            get { return "Profesional"; }
        }

        public virtual long Matricula { get; set; }
    }
}