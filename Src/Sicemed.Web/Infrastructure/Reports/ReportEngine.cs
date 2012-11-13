using System;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Sicemed.Web.Infrastructure.Reports
{
    public class ReportEngine
    {
        private const string PATH_REPORTS = "~/Reports/";
        private const string FILE_EXTENSION_REPORTS = "rdlc";

        public byte[] BuildReport<T>(IReport<T> report, ReportOutputType reportOutputType = ReportOutputType.PDF)
        {
            var localReport = new LocalReport();
            localReport.ReportPath = HttpContext.Current.Server.MapPath(String.Format("{0}{1}.{2}", PATH_REPORTS, report.Name, FILE_EXTENSION_REPORTS));  // "~/Content/Reports/CustomerReport.rdlc");
            var reportDataSource = new ReportDataSource(report.DataSource, report.Execute());
            localReport.DataSources.Add(reportDataSource);
            localReport.DisplayName = report.Title;            

            localReport.SetParameters(report.Parameters.Select(p => new ReportParameter(p.Key, p.Value)));

            var reportType = reportOutputType.ToString().ToUpper();
            var deviceInfo = GetDeviceInfo(reportOutputType);

            string mimeType;
            string encoding;
            string fileNameExtension;

            Warning[] warnings;
            string[] streams;

            //Render the report
            return localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }

        private string GetDeviceInfo(ReportOutputType reportOutputType)
        {
            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            switch (reportOutputType)
            {
                case ReportOutputType.PDF:
                    //A4 Page
                    return "<DeviceInfo>" +
                            "  <OutputFormat>PDF</OutputFormat>" +
                            "  <PageWidth>21cm</PageWidth>" +
                            "  <PageHeight>29.7cm</PageHeight>" +
                            "  <MarginTop>1cm</MarginTop>" +
                            "  <MarginLeft>2cm</MarginLeft>" +
                            "  <MarginRight>1cm</MarginRight>" +
                            "  <MarginBottom>1cm</MarginBottom>" +
                            "</DeviceInfo>";

                case ReportOutputType.CSV:
                    return "<DeviceInfo>" +
                                         "  <OutputFormat>CSV</OutputFormat>" +
                                         "<Encoding>UTF-8</Encoding>" +
                                         "<ExcelMode>true</ExcelMode>" +
                                         "</DeviceInfo>";
            }

            throw new ArgumentOutOfRangeException("reportOutputType");
        }
    }
}