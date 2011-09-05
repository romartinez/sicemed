namespace Sicemed.Web.Models
{
    public class EspecialidadProfesional : Entity
    {
        #region Navigation Properties        

        public virtual Especialidad Especialidad { get; set; }

        public virtual Usuario Profesional { get; set; }

        #endregion
    }
}