using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;

namespace NBSF.Fop.Parameters
{
	/// <summary>
	/// Summary description for FontConfigurationHandler.
	/// </summary>
	public class ParameterConfigurationHandler : System.Configuration.IConfigurationSectionHandler
	{
        public ParameterConfigurationHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public object Create(object parent, object configContext, XmlNode section) 
		{
            List<ParameterConfiguration> pcl = new List<ParameterConfiguration>();
            string name = string.Empty;
            string value = string.Empty;
			foreach(XmlElement n in section.SelectNodes("parameter")) 
			{
                name = n.GetAttribute("name");
                value = ResolvePath(n.GetAttribute("value"));
                pcl.Add(new ParameterConfiguration(name, value, n.GetAttribute("NamespaceURI")));
            }
			return pcl;
		}
    
        private string ResolvePath(string ruta)
        {
            HttpContext context = null;
            if (ruta.StartsWith("~"))
            {
                context = HttpContext.Current;
                if (context != null) //Estoy en una aplicacion web
                {
                    ruta = ruta.Replace("~", context.Server.MapPath("~"));
                }
            }
            return ruta;
        }
    }
}
