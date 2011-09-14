namespace Sicemed.Web.Models
{
    public class EspecialidadProfesional : Entity
    {
        #region Navigation Properties        

        public virtual Especialidad Especialidad { get; set; }

        public virtual Persona Profesional { get; set; }

        #endregion
    }
}