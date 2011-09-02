using System;

namespace Sicemed.Web.Models
{
    [Serializable]
    public abstract class Entity : IEquatable<Entity>
    {
        // Methods

        public virtual long Id { get; protected internal set; }

        #region IEquatable<Entity> Members

        public virtual bool Equals(Entity obj)
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
            return Equals((Entity) obj);
        }

        public override int GetHashCode()
        {
            return ((Id.GetHashCode()*0x18d) ^ base.GetType().GetHashCode());
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }

        // Properties
    }
}