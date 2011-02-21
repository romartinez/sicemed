using System.Configuration;
using System.Text;
using java.io;
using NBSF.Fop.Fonts;
using org.apache.fop.apps;
using org.apache.fop.configuration;
using org.xml.sax;
using System.Collections.Generic;

namespace NBSF.Fop
{
	/// <summary>
	/// Summary description for InternalEngine.
	/// </summary>
	
	/// <summary>
	/// Esta clase es para ocultar todo lo referente a J# y ApacheFop.NET
	/// </summary>
	internal class InternalEngine
	{
		internal static void Run(InputSource source, OutputStream output) 
		{
			InternalEngine.LoadFonts();
            Driver drvr1 = new Driver(source, output);
            drvr1.setRenderer(Driver.RENDER_PDF);
            drvr1.run();
		}

		private static void LoadFonts() 
		{
			StringBufferInputStream sbis = null;
			ConfigurationReader xr = null;
			StringBuilder fonts = null;
            List<FontConfiguration> fcl = (List<FontConfiguration>)System.Configuration.ConfigurationManager.GetSection("nbsf.fop.fonts");

			if ( fcl != null ) 
			{
				fonts = new StringBuilder();
				foreach(FontConfiguration fc in fcl) fonts.Append("<font metrics-file=\"file://" + fc.MetricsFile + "\" kerning=\"" + fc.Kerning + "\" embed-file=\"file://" + fc.EmbedFile + "\"><font-triplet name=\"" + fc.Name + "\" style=\"" + fc.Style + "\" weight=\"" + fc.Weight + "\"/></font>");
				sbis = new StringBufferInputStream(string.Format("<configuration><fonts>{0}</fonts></configuration>", fonts.ToString() ) );
				xr = new ConfigurationReader(new InputSource(sbis));
				xr.start();
			}
		}	

	}
}
