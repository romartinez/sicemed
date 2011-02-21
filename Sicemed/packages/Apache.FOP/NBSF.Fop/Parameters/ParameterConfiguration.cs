using System;

namespace NBSF.Fop.Parameters
{
	/// <summary>
	/// Summary description for FontConfiguration.
	/// </summary>
	public class ParameterConfiguration
	{
		internal string _name = string.Empty;
        internal string _value = string.Empty;
        internal string _namespaceuri = string.Empty;

		internal ParameterConfiguration() 
		{
		}

        internal ParameterConfiguration(string name, string value, string namespaceuri)
        {
            _name = name;
            _value = value;
            _namespaceuri = namespaceuri;
        }

        internal ParameterConfiguration(string name, string value) : this(name, value, string.Empty)
		{
		}

		public string Name 
		{
			get { return _name; }
		}

        public string Value
        {
            get { return _value; }
        }

        public string NamespaceURI
        {
            get { return _namespaceuri; }
        }

		public override int GetHashCode() 
		{
            return string.Format("NBSF.Fop.Parameters.ParameterConfiguration_{0}", this.Name).GetHashCode();
		}

		public override bool Equals(object param) 
		{
            if (param as ParameterConfiguration == null) throw new ArgumentException("Esta implementacion solo soporta objetos del tipo ParameterConfiguration");
            ParameterConfiguration f = param as ParameterConfiguration;
			return ( f.Name == this.Name);
		}
	}
}
