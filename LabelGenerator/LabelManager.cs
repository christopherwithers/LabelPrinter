using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Extensions;
using LabelGenerator.Interfaces;
using NLog;

//Make internals visible to testing framework
#if DEBUG
[assembly: InternalsVisibleTo("LabelPrinter.Tests")]
#endif
namespace LabelGenerator
{
    public class LabelManager : ILabelManager
    {
        internal XmlDocument Document;

        private readonly IFileManager _fileManager;

        public LabelManager(IFileManager fileManager)
        {
            _fileManager = fileManager;

            Log = LogManager.GetCurrentClassLogger();
        }

        public ILogger Log { get; set; }

        //todo: should we refactor this to an IDocument manager?? Or just stub out for unit testing??
        internal async Task<bool> SourceLocation(string location)
        {

            var success = true;

            var xml = await _fileManager.ReadXMLFile(location);

            try
            {
                Document = new XmlDocument();

                Document.LoadXml(xml);
            }
            catch (Exception ex)
            {
                success = false;
                Document = null;

                Log.Error(ex);
            }

            return success;
        }



        public virtual async Task<Dictionary<string, string>> ParseLabelFromFile(string fileLocation)
        {
            if (string.IsNullOrEmpty(fileLocation))
            {
                Log.Error("The location string is empty.");

                return null;
            }

            if (!fileLocation.EndsWith(".xml"))
            {
                Log.Error($"The location string '{fileLocation}' is not an xml document.");

                return null;
            }


            if (!await SourceLocation(fileLocation))
            {
                Log.Error($"The location string '{fileLocation}' is invalid");

                return null;
            }

            if (Document?.DocumentElement == null)
            {
                Log.Error($"The generated XML from '{fileLocation}' is malformed. The 'DocumentElement' is null.");

                return null;
            }

            var xDoc = ConvertXmlDocumentToXDocument(Document);

            if (xDoc == null)
            {
                Log.Error($"The XDocument conversion from '{fileLocation}' failed.");

                return null;
            }

            var xsdDoc = await _fileManager.ReadXMLFile($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\LabelFile.xsd");


            if (string.IsNullOrEmpty(xsdDoc))
            {
                Log.Error(@"The LabelFile.xsd XSD file could not be loaded from the \Config directory.");

                return null;
            }

            if (!ValidateXml(xDoc, xsdDoc))
            {
                Log.Error($"The XML from '{fileLocation}' does not validate againt the XSD document.");

                return null;
            }

            var outputDict = new Dictionary<string, string>();

            foreach (var element in xDoc.Root.Elements())
                {
                    RecParseXDoc(element, outputDict);
                }

            return outputDict;
        }

        internal bool ValidateXml(XDocument document, string xsdDocument)
        {
            var schemas = new XmlSchemaSet();

            var valid = true;
            try
            {
                //   var xsdDoc = );

                schemas.Add("", XmlReader.Create(new StringReader(xsdDocument)));
                var xrs = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                xrs.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                xrs.Schemas = schemas;




                document?.Document?.Validate(schemas, (o, e) =>
                {
                    // Console.WriteLine("{0}", e.Message);
                    valid = false;
                });
            }
            catch (XmlSchemaValidationException)
            {
                valid = false;
            }
            catch (Exception)
            {
                valid = false;
            }


            return valid;
        }



        internal virtual XDocument ConvertXmlDocumentToXDocument(XmlDocument xmlDocument)
        {
            XDocument xDoc = null;

            try
            {
                xDoc = xmlDocument.ToXDocument();

                if (xDoc.Root == null)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return xDoc;
        }

        private static void RecParseXDoc(XElement element, IDictionary<string, string> xDict)
        {
            if (element.HasElements)
            {
                foreach (var childElement in element.Elements())
                {
                    RecParseXDoc(childElement, xDict);
                }

            }
            xDict.Add(element.Name.ToString(), element.Value);
        }
    }
}
