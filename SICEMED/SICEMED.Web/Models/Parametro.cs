namespace Sicemed.Web.Models
{
    public class Parametro : Entity
    {
        public enum Keys
        {
            MIN_PASSWORD_LENGTH            
        }

        #region Primitive Properties

        public virtual string Key { get; set; }

        public virtual string Value { get; set; }

        #endregion
    }
}