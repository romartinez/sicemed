using System;
using System.Xml;
using System.Web;
using System.Collections.Generic;

namespace NBSF.Fop.Fonts
{
	/// <summary>
	/// Summary description for FontConfigurationHandler.
	/// </summary>
	public class FontConfigurationHandler : System.Configuration.IConfigurationSectionHandler
	{
		public FontConfigurationHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public object Create(object parent, object configContext, XmlNode section) 
		{
            List<FontConfiguration> fcl = new List<FontConfiguration>();
            string metricsFile = string.Empty;
            string embedFile = string.Empty;
			foreach(XmlElement n in section.SelectNodes("font")) 
			{
                metricsFile = ResolvePath(n.GetAttribute("metrics_file"));
                embedFile = ResolvePath(n.GetAttribute("embed_file"));
				fcl.Add(new FontConfiguration(metricsFile, n.GetAttribute("kerning"), embedFile, n.GetAttribute("name"), n.GetAttribute("style"), n.GetAttribute("weight") ) );
			}
			return fcl;
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
