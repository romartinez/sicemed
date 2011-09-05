using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sicemed.Web.Models.Enumerations
{
    public abstract class Enumeration : IComparable
    {
        private readonly long _value;
        private readonly string _displayName;

        protected Enumeration()
        {
        }

        protected Enumeration(long value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public virtual long Value
        {
            get { return _value; }
        }

        public virtual string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach(var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if(locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if(otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static long AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if(matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ApplicationException(message);
            }

            return matchingItem;
        }

        public virtual int CompareTo(object other)
        {
            return Value.CompareTo(((Enumeration)other).Value);
        }

        public static bool operator ==(Enumeration a, Enumeration b)
        {
            if((object)a == null && (object)b == null) return true;
            if((object)a == null || (object)b == null) return false;
            return a.Value == b.Value;
        }

        public static bool operator !=(Enumeration a, Enumeration b)
        {
            return !(a == b);
        }
    }
}