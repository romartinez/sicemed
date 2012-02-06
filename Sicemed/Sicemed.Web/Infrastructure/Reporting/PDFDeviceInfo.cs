using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Sicemed.Web.Infrastructure.Reporting
{
    /// <summary>
    /// Representa las opcioines de generación de PDF de la clase LocalReport segúin documentación en 
    /// http://msdn.microsoft.com/en-us/library/ms154682.aspx
    /// </summary>
    [XmlRoot("DeviceInfo")]
    public class PDFDeviceInfo
    {
        public static PDFDeviceInfo Default()
        {
            var deviceInfo = new PDFDeviceInfo();

            deviceInfo.PageHeight = "8.5in";
            deviceInfo.PageWidth = "11in";
            deviceInfo.MarginTop = "0.5in";
            deviceInfo.MarginRight = "1in";
            deviceInfo.MarginLeft = "1in";
            deviceInfo.MarginBottom = "0.5in";

            return deviceInfo;
        }


        /// <summary>
        /// The number of columns to set for the report. This value overrides the report's original settings.
        /// </summary>
        public string Columns { get; set; }

        /// <summary>
        /// The column spacing to set for the report. This value overrides the report's original settings.
        /// </summary>        
        public string ColumnSpacing { get; set; }

        /// <summary>
        /// The last page of the report to render. The default value is the value for StartPage.
        /// </summary>
        public string EndPage { get; set; }

        /// <summary>
        /// Indicates whether the PDF should be compressed, which allows the source to be more readable. The default value is false.
        /// </summary>
        public string HumanReadablePDF { get; set; }

        /// <summary>
        /// The bottom margin value, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 1in). This value overrides the report's original settings.
        /// </summary>
        public string MarginBottom { get; set; }

        /// <summary>
        /// The left margin value, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 1in). This value overrides the report's original settings.
        /// </summary>
        public string MarginLeft { get; set; }

        /// <summary>
        /// The right margin value, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 1in). This value overrides the report's original settings.
        /// </summary>
        public string MarginRight { get; set; }

        /// <summary>
        /// The top margin value, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 1in). This value overrides the report's original settings.
        /// </summary>
        public string MarginTop { get; set; }

        /// <summary>
        /// The page height, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 11in). This value overrides the report's original settings.
        /// </summary>
        public string PageHeight { get; set; }

        /// <summary>
        /// The page width, in inches, to set for the report. You must include an integer or decimal value followed by "in" (for example, 8.5in). This value overrides the report's original settings.
        /// </summary>
        public string PageWidth { get; set; }

        /// <summary>
        /// The first page of the report to render. A value of 0 indicates that all pages are rendered. The default value is 1.
        /// </summary>
        public string StartPage { get; set; }

        public override string ToString()
        {
            var stream = new MemoryStream();

            var serializer = new XmlSerializer(this.GetType(), "");

            serializer.Serialize(stream, this);

            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
    }
}