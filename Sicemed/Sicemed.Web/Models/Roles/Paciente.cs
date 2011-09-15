namespace Sicemed.Web.Models.Roles
{
    public class Paciente : Rol
    {
        public override string DisplayName
        {
            get { return PACIENTE; }
        }

        public virtual string NumeroAfiliado { get; set; }

        public virtual int InasistenciasContinuas { get; set; }

        public virtual bool EstaHabilitadoTurnosWeb { get; set; }

        protected Paciente() { }

        public static Rol Create(string numeroAfiliado)
        {
            return new Paciente()
                   {
                       NumeroAfiliado = numeroAfiliado, 
                       EstaHabilitadoTurnosWeb = true, 
                       InasistenciasContinuas = 0
                   };
        }

    }
}