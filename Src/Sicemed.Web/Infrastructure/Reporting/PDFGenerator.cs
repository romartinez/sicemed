//using System.Collections.Generic;
//using System.Dynamic;
//using System.IO;
//using System.Windows.Forms;
//using Microsoft.Reporting.WebForms;

//namespace Sicemed.Web.Infrastructure.Reporting
//{
//    public class PDFGenerator
//    {
//        public byte[] OnTheFlyRender(object data, PDFDeviceInfo deviceInfo)
//        {
//            var lr = new LocalReport();

//            if (deviceInfo == null)
//                deviceInfo = PDFDeviceInfo.Default();

//            var reportDefinition = File.OpenRead(@"D:\Documents\Projects\sicemed\Sicemed\Sicemed.Web\Reports\ComprobanteTurno.rdlc");

//            lr.LoadReportDefinition(reportDefinition); //el rdlc
//            var parameters = new List<ReportParameter>();
//            //parameters.Add(new ReportParameter("NombreDataSourceEnElReporte", dataSourceValue));

//            lr.EnableExternalImages = true;
//            lr.DataSources.Add(new ReportDataSource("Dato", new BindingSource() { DataSource = data }));

//            lr.SetParameters(parameters);

//            lr.Refresh();

//            Warning[] warnings;
//            string[] streamids;
//            string mimeType;
//            string encoding;
//            string extension;

//            byte[] bytes = lr.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);

//            return bytes;
//        }         
//    }
//}