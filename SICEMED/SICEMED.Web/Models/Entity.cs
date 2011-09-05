using System;

namespace Sicemed.Web.Models
{
    [Serializable]
    public abstract class Entity : IEquatable<Entity>
    {
        // Methods

        public virtual long Id { get; protected internal set; }

        private int? _requestedHashCode;

        /// <summary>
        /// Compare equality trough Id
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns>true is are equals</returns>
        /// <remarks>
        /// Two entities are equals if they are of the same hierarcy tree/sub-tree
        /// and has same id.
        /// </remarks>
        public virtual bool Equals(Entity other)
        {
            if (null == other || !GetType().IsInstanceOfType(other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            bool otherIsTransient = Equals(other.Id, default(long));
            bool thisIsTransient = IsTransient();
            if (otherIsTransient && thisIsTransient)
            {
                return ReferenceEquals(other, this);
            }

            return other.Id.Equals(Id);
        }

        protected bool IsTransient()
        {
            return Equals(Id, default(long));
        }

        public override bool Equals(object obj)
        {
            var that = obj as Entity;
            return Equals(that);
        }

        public override int GetHashCode()
        {
            if (!_requestedHashCode.HasValue)
            {
                _requestedHashCode = IsTransient() ? base.GetHashCode() : Id.GetHashCode();
            }
            return _requestedHashCode.Value;
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