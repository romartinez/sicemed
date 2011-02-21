using System;

namespace NBSF.Fop.Fonts
{
	/// <summary>
	/// Summary description for FontConfiguration.
	/// </summary>
	public class FontConfiguration
	{
		internal string _name = string.Empty;
		internal string _style = string.Empty;
		internal string _weight = string.Empty;

		internal string _metrics_file = string.Empty;
		internal string _kerning = string.Empty;
		internal string _embed_file = string.Empty;

		internal FontConfiguration() 
		{
		}

		internal FontConfiguration(string metrics_file, string kerning, string embed_file, string name, string style, string weight)
		{
			_name = name;
			_style = style;
			_weight = weight;
			_metrics_file = metrics_file;
			_kerning = kerning;
			_embed_file = embed_file;
		}

		public string Name 
		{
			get { return _name; }
		}

		public string Style 
		{
			get { return _style; }
		}

		public string Weight
		{
			get { return _weight; }
		}

		public string MetricsFile 
		{
			get { return _metrics_file; }
		}

		public string Kerning 
		{
			get { return _kerning; }
		}

		public string EmbedFile 
		{
			get { return _embed_file; }
		}

		public override int GetHashCode() 
		{
			return string.Format("NBSF.Fop.Fonts.FontConfiguration_{0}_{1}", this.EmbedFile, this.MetricsFile).GetHashCode();
		}

		public override bool Equals(object font) 
		{
			if ( font as FontConfiguration == null ) throw new ArgumentException("Esta implementacion solo soporta objetos del tipo FontConfiguration");
			FontConfiguration f = font as FontConfiguration;
			return ( f.MetricsFile == this.MetricsFile ) && ( this.EmbedFile == f.EmbedFile );
		}
	}
}
