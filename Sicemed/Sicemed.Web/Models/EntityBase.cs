using System;

namespace Sicemed.Web.Models
{
    [Serializable]
    public abstract class EntityBase : IEquatable<EntityBase>
    {
        // Methods

        public virtual long Id { get; private set; }

        #region IEquatable<EntityBase> Members

        public virtual bool Equals(EntityBase obj)
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
            return (obj.Id == Id);
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
            return Equals((EntityBase) obj);
        }

        public override int GetHashCode()
        {
            return ((Id.GetHashCode()*0x18d) ^ base.GetType().GetHashCode());
        }

        public static bool operator ==(EntityBase left, EntityBase right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EntityBase left, EntityBase right)
        {
            return !Equals(left, right);
        }

        // Properties
    }
}