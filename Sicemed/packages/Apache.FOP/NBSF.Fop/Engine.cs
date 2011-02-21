using System.IO;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using java.io;
using org.apache.fop.apps;
using org.xml.sax;
using System.Collections.Generic;
using NBSF.Fop.Parameters;

namespace NBSF.Fop
{
	/// <summary>
	/// Summary description for Engine.
	/// </summary>
	public class Engine
	{
        public static string Run64(XmlTextReader xmlrr, string xsl)
        {
            string pdf = System.IO.Path.GetTempFileName();
            using (XmlTextReader xslrr = new XmlTextReader(xsl))
            {
                Engine.Run(xmlrr, xslrr, null, pdf, Encoding.GetEncoding("UTF-8"));
            }
            if (!System.IO.File.Exists(pdf)) throw new System.ApplicationException("No se encuentra el archivo PDF que debería haber generado");
            return GetBase64File(pdf);
        }

        public static string Run64(XmlDocument xmldoc, string xsl)
        {
            string pdf = System.IO.Path.GetTempFileName();
            Engine.Run(xmldoc, xsl, null, pdf, Encoding.GetEncoding("UTF-8"));
            if (!System.IO.File.Exists(pdf)) throw new System.ApplicationException("No se encuentra el archivo PDF que debería haber generado");
            return GetBase64File(pdf);
        }

        public static string Run64(string xml, string xsl)
        {
            string pdf = System.IO.Path.GetTempFileName();
            Engine.Run(xml, xsl, null, pdf, Encoding.GetEncoding("UTF-8"));
            if (!System.IO.File.Exists(pdf)) throw new System.ApplicationException("No se encuentra el archivo PDF que debería haber generado");
            return GetBase64File(pdf);
        }

        public static string GetBase64File(string path)
        {
            FileStream fs = null;
            byte[] b = null;
            int i = 0;

            try
            {
                fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                b = new byte[fs.Length];
                while (fs.Position != fs.Length) b[i++] = (byte)fs.ReadByte();
                return System.Convert.ToBase64String(b);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

		public static void Run(string xml, string xsl, string encoding, string pdf)
		{
			Engine.Run(xml, xsl, null, pdf, Encoding.GetEncoding(encoding));
		}

		public static void Run(string xml, string xsl, string pdf)
		{
			Engine.Run(xml, xsl, null, pdf, Encoding.GetEncoding("UTF-8"));
		}

        public static void Run(string xml, string xsl, XsltArgumentList xslarg, string pdf, Encoding enc)
        {
            using (XmlTextReader xmlrr = new XmlTextReader(xml))
                using (XmlTextReader xslrr = new XmlTextReader(xsl))
                    Run(xmlrr, xslrr, xslarg, pdf, enc);
        }

        public static void Run(XmlDocument xmldoc, string xsl, XsltArgumentList xslarg, string pdf, Encoding enc)
        {
            using (XmlTextReader xslrr = new XmlTextReader(xsl))
                Run(new XmlTextReader(new System.IO.StringReader(xmldoc.InnerXml)), xslrr, xslarg, pdf, enc);
        }

        public static void Run(XmlTextReader xmlrr, XmlTextReader xslrr, XsltArgumentList xslarg, string pdf, Encoding enc) 
		{
			sbyte[] sby = null;
			byte[] by = null;
            List<ParameterConfiguration> pcl = null;
            XslCompiledTransform xslt = new XslCompiledTransform();
			MemoryStream ms = new MemoryStream();
			XmlTextWriter xw = new XmlTextWriter(ms, enc);
            XsltSettings settings = new XsltSettings();
            settings.EnableScript = true;
            settings.EnableDocumentFunction = true;

			XmlResolver resolver = new XmlUrlResolver();
			System.Security.Policy.Evidence evidence = new System.Security.Policy.Evidence();
			evidence.AddHost(new Zone(SecurityZone.Trusted));

            xslt.Load(xslrr, settings, resolver);

            if (xslarg == null) xslarg = new XsltArgumentList();
            pcl = (List<ParameterConfiguration>)System.Configuration.ConfigurationManager.GetSection("nbsf.fop.parameters");
            if ( pcl != null ) foreach (ParameterConfiguration pc in pcl) xslarg.AddParam(pc.Name, pc.NamespaceURI, pc.Value);
            
			xslt.Transform(xmlrr, xslarg, xw, resolver);
			sby = new sbyte[ms.Length];
			by = ms.ToArray();
			ms.Close();
			xw.Close();
			for(int i = 0; i < by.Length; i++) sby[i] = (sbyte)(short)by[i]; // C# -> J#

			FileOutputStream str2 = new FileOutputStream(pdf);
			ByteArrayInputStream bis = new ByteArrayInputStream(sby);
			InputSource isrc = new InputSource(bis);
			InternalEngine.Run(isrc, str2);
			str2.close();
		}

		public static void Run(string fo, string pdf) 
		{
			FileOutputStream str2 = new FileOutputStream(pdf);
			InternalEngine.Run(new InputSource(new FileInputStream(fo)), str2);
			str2.close();
		}

		public static void Run(StreamReader fo, StreamWriter sw) 
		{
			Engine.Run(fo, sw, false);
		}

		public static void Run(StreamReader fo, StreamWriter sw, bool closestream)  
		{
			Engine.Run(fo, sw.BaseStream);
			if ( closestream ) sw.Close();
		}

		public static void Run(StreamReader fo, Stream sm)  
		{
			ByteArrayOutputStream bout = new ByteArrayOutputStream();
			InternalEngine.Run(new InputSource(new StringBufferInputStream(fo.ReadToEnd())), bout);
			foreach(sbyte s in bout.toByteArray()) sm.WriteByte((byte)(short)s); //J# -> C#
			bout.close();
		}
	}
}
