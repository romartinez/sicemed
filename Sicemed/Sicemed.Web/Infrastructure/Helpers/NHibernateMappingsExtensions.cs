using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using NHibernate.Cfg.MappingSchema;

namespace Sicemed.Web.Infrastructure.Helpers
{
    /// <summary>
    ///   Util extensions to use in your test or where you need to see the XML mappings
    /// </summary>
    /// <remarks>
    ///   You can copy&paste this source in your tests and create a test to create/see your XML mappings.
    ///   Mappings are created per class (each class in its XML) in a folder named "Mappings" inside the "bin" folder.
    /// </remarks>
    public static class NHibernateMappingsExtensions
    {
        public static void WriteAllXmlMapping(this IEnumerable<HbmMapping> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }
            var mappingsFolderPath = ArrangeMappingsFolderPath();
            foreach (var hbmMapping in mappings)
            {
                var fileName = GetFileName(hbmMapping);
                var document = Serialize(hbmMapping);
                File.WriteAllText(Path.Combine(mappingsFolderPath, fileName), document);
            }
        }

        public static string AsString(this HbmMapping mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException("mappings");
            }
            return Serialize(mappings);
        }

        private static string ArrangeMappingsFolderPath()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var relativeSearchPath = AppDomain.CurrentDomain.RelativeSearchPath;
            var binPath = relativeSearchPath != null ? Path.Combine(baseDir, relativeSearchPath) : baseDir;
            var mappingsFolderPath = Path.Combine(binPath, "Mappings");

            if (!Directory.Exists(mappingsFolderPath))
            {
                Directory.CreateDirectory(mappingsFolderPath);
            } else
            {
                Array.ForEach(Directory.GetFiles(mappingsFolderPath), File.Delete);
            }
            return mappingsFolderPath;
        }

        private static string GetFileName(HbmMapping hbmMapping)
        {
            var name = "MyMapping";
            var rc = hbmMapping.RootClasses.FirstOrDefault();
            if (rc != null)
            {
                name = rc.Name;
            }
            var sc = hbmMapping.SubClasses.FirstOrDefault();
            if (sc != null)
            {
                name = sc.Name;
            }
            var jc = hbmMapping.JoinedSubclasses.FirstOrDefault();
            if (jc != null)
            {
                name = jc.Name;
            }
            var uc = hbmMapping.UnionSubclasses.FirstOrDefault();
            if (uc != null)
            {
                name = uc.Name;
            }
            return name + ".hbm.xml";
        }

        private static string Serialize(HbmMapping hbmElement)
        {
            string result;
            var setting = new XmlWriterSettings {Indent = true};
            var serializer = new XmlSerializer(typeof (HbmMapping));
            using (var memStream = new MemoryStream(2048))
            {
                using (var xmlWriter = XmlWriter.Create(memStream, setting))
                {
                    serializer.Serialize(xmlWriter, hbmElement);
                }
                memStream.Position = 0;
                using (var sr = new StreamReader(memStream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }
    }
}