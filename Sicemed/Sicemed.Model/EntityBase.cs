using System;

namespace Sicemed.Model {
    [Serializable]
    public abstract class EntityBase : IEquatable<EntityBase> {

        // Methods

        public virtual bool Equals(EntityBase obj) {
            if (object.ReferenceEquals(null, obj)) {
                return false;
            }
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            if (base.GetType() != obj.GetType()) {
                return false;
            }
            return (obj.Id == this.Id);
        }

        public override bool Equals(object obj) {
            if (object.ReferenceEquals(null, obj)) {
                return false;
            }
            if (object.ReferenceEquals(this, obj)) {
                return true;
            }
            if (base.GetType() != obj.GetType()) {
                return false;
            }
            return this.Equals((EntityBase)obj);
        }

        public override int GetHashCode() {
            return ((this.Id.GetHashCode() * 0x18d) ^ base.GetType().GetHashCode());
        }

        public static bool operator ==(EntityBase left, EntityBase right) {
            return object.Equals(left, right);
        }

        public static bool operator !=(EntityBase left, EntityBase right) {
            return !object.Equals(left, right);
        }

        // Properties
        public virtual long Id {
            get;
            private set;
        }
    }

}