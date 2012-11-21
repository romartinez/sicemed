//using System.IO;
//using NUnit.Framework;
//using Sicemed.Web.Infrastructure.Reporting;

//namespace Sicemed.Tests.Infrastructure.Reporting
//{
//    [TestFixture]
//    public class PDFGeneratorTests
//    {
//        [Test]
//        [Ignore("No se está usando el Reporting Services Local")]
//        public void CanCreateAnEmptyReport()
//        {
//            var g = new PDFGenerator();
//            var b = g.OnTheFlyRender(null, null);

//            File.WriteAllBytes(@"C:\test.pdf", b);
//        }        
        
//        [Test]
//        [Ignore("No se está usando el Reporting Services Local")]
//        public void CanCreateReport()
//        {
//            var g = new PDFGenerator();
//            var b = g.OnTheFlyRender("RM", null);

//            File.WriteAllBytes(@"C:\test2.pdf", b);
//        }
         
//    }
//}