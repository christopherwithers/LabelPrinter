using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Extensions;
using LabelGenerator.Interfaces;

namespace LabelGenerator
{
    public class LabelManager : ILabelManager
    {
        private XmlDocument _document;

        //todo: should we refactor this to an IDocument manager?? Or just stub out for unit testing??
        internal bool SourceLocation(string location)
        {
            var success = true;

            try
            {
                using (var r = new StreamReader(location))
                {
                    string currentLine;
                    var xmlDoc = new StringBuilder();
                    while ((currentLine = r.ReadLine()) != null)
                        xmlDoc.Append(currentLine.Trim());

                    _document = new XmlDocument();
                    _document.LoadXml(xmlDoc.ToString());
                }
            }
            catch (Exception)
            {
                success = false;
                _document = null;
            }

            return success;
        }

        public Dictionary<string, string> ParseLabelFromFile(string fileLocation)
        {
            if (!SourceLocation(fileLocation))
                return null;

            if (_document?.DocumentElement == null)
                return null;

            var xDoc = _document.ToXDocument();

            if (xDoc.Root == null)
                return null;

            var outputDict = new Dictionary<string, string>();

            foreach (var element in xDoc.Root.Elements())
            {
                RecParseXDoc(element, outputDict);
            }


            return outputDict;
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
