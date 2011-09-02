using System;
using Newtonsoft.Json;

namespace Sicemed.Web.Models
{
    public class Parametro : IEquatable<Parametro>
    {
        public virtual Keys Key { get; set; }
        private string _value;

        public virtual T Get<T>()
        {
            return JsonConvert.DeserializeObject<T>(_value);
        }

        public virtual void Set(object item)
        {
            _value = JsonConvert.SerializeObject(item);
        }

        public enum Keys
        {
            APP_IS_INITIALIZED
        }

        #region IEquatable<Parameter> Members

        public virtual bool Equals(Parametro obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (base.GetType() != obj.GetType())
            {
                return false;
            }
            return (obj.Key == Key);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (base.GetType() != obj.GetType())
            {
                return false;
            }
            return Equals((Parametro)obj);
        }

        public override int GetHashCode()
        {
            return ((Key.GetHashCode() * 0x18d) ^ base.GetType().GetHashCode());
        }

        public static bool operator ==(Parametro left, Parametro right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Parametro left, Parametro right)
        {
            return !Equals(left, right);
        }
    }
}