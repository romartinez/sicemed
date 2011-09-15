namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        public override string DisplayName
        {
            get { return "Paciente"; }
        }

        public virtual string NumeroAfiliado { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual bool EstaHabilitadoTurnosWeb { get; set; }
    }
}