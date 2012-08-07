using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace Sicemed.Web.Infrastructure.Reports
{
    public class ServiceReport
    {
        private static List<ReportInfo> _reports = new List<ReportInfo>();
        private static string dirCsv = System.Web.HttpContext.Current.Server.MapPath(@"~\CSV");
        public Dictionary<string, string> typesMime = new Dictionary<string, string>();

        private const string PathReports = "~/Reports/";
        private const string FileExtensionReports = "rdlc";


        public ServiceReport()
        {
            typesMime.Add("pdf", "application/pdf");
            typesMime.Add("csv", "text/csv");
        }

        public byte[] BuildReport(ReportInfo reportInfo, string typeOutput)
        {
            var localReport = new LocalReport();
            localReport.ReportPath = HttpContext.Current.Server.MapPath(String.Format("{0}{1}.{2}", PathReports, reportInfo.Key, FileExtensionReports));  // "~/Content/Reports/CustomerReport.rdlc");
            var reportDataSource = new ReportDataSource(reportInfo.NameDataSource, reportInfo.DataSource);
            localReport.DataSources.Add(reportDataSource);

            string reportType = typeOutput.ToUpper();
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx

            string deviceInfoPdf =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            string deviceInfoCsv =
            "<DeviceInfo>" +
            "  <OutputFormat>CSV</OutputFormat>" +
            "<Encoding>UTF-8</Encoding>" +
            "<ExcelMode>true</ExcelMode>" +
            "</DeviceInfo>";

            string deviceInfo;

            if (typeOutput.ToUpper() == "PDF")
                deviceInfo = deviceInfoPdf;
            else
                deviceInfo = deviceInfoCsv;



            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            return localReport.Render(reportType, deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
        }         
    }
}