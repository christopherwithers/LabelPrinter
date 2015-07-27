using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator
{
    public class XmlSourceParser : ISourceParser
    {
        private XmlDocument _document;

        internal bool SourceLocation(string location)
        {
            _document = new XmlDocument();

            var success = true;

            try
            {
                _document.Load(location);
            }
            catch (Exception)
            {
                success = false;
                _document = null;
            }
            
            return success;
        }

        public LabelItem GenerateLabelItem(string location)
        {

            if (!SourceLocation(location))
                return null;

            if (_document == null)
                return null;

            if (_document.DocumentElement == null) 
                return null;


            var labelItem = new LabelItem
            {
                FormName = ParseXmlNode(_document.DocumentElement, "form-name"),
                FormLanguage = ParseXmlNode(_document.DocumentElement, "form-language"),
                FormFormat = ParseXmlNode(_document.DocumentElement, "form-format"),
                Subject = ParseXmlNode(_document.DocumentElement, "subject")
            };

            var xmlSectionNodes = _document.DocumentElement.SelectNodes(@"//*[contains(name(),'section-')]");

            labelItem.Sections = ParseSections(xmlSectionNodes);

            return labelItem;
        }

        private static Collection<Section> ParseSections(IEnumerable nodeList)
        {
            var sections = new Collection<Section>();

            if (nodeList == null) 
                return sections;

            foreach (XmlNode node in nodeList)
            {
                var section = new Section
                {
                    Name = node.LocalName,
                    FormDate = ParseXmlNode(node, "form-date"),
                    BibInfo = ParseXmlNode(node, "bib-info"),
                    Z13 =
                    {
                        DocNumber = ParseXmlNode(node, "z13-doc-number"),
                        Year = ParseXmlNode(node, "z13-year"),
                        OpenDate = ParseXmlNode(node, "z13-update-date"),
                        UpdateDate = ParseXmlNode(node, "z13-open-date"),
                        CallNoKey = ParseXmlNode(node, "z13-call-no-key"),
                        CallNoCode = ParseXmlNode(node, "z13-call-no-code"),
                        CallNo = ParseXmlNode(node, "z13-call-no"),
                        AuthorCode = ParseXmlNode(node, "z13-author-code"),
                        Author = ParseXmlNode(node, "z13-author"),
                        TitleCode = ParseXmlNode(node, "z13-title-code"),
                        Title = ParseXmlNode(node, "z13-title"),
                        ImprintCode = ParseXmlNode(node, "z13-imprint-code"),
                        Imprint = ParseXmlNode(node, "z13-imprint"),
                        IsbnIssnCode = ParseXmlNode(node, "z13-isbn-issn-code"),
                        IsbnIssn = ParseXmlNode(node, "z13-isbn-issn"),
                        UpdTimeStamp = ParseXmlNode(node, "z13-upd-time-stamp")
                    },
                    Z30 =
                    {
                        DocNumber = ParseXmlNode(node, "z30-doc-number"),
                        ItemSequence = ParseXmlNode(node, "z30-item-sequence"),
                        BarCode = ParseXmlNode(node, "z30-barcode"),
                        SubLibrary = ParseXmlNode(node, "z30-sub-library"),
                        Material = ParseXmlNode(node, "z30-material"),
                        ItemStatus = ParseXmlNode(node, "z30-item-status"),
                        OpenDate = ParseXmlNode(node, "z30-open-date"),
                        UpdateDate = ParseXmlNode(node, "z30-update-date"),
                        Cataloger = ParseXmlNode(node, "z30-cataloger"),

                        DateLastReturn = ParseXmlNode(node, "z30-date-last-return"),
                        HourLastReturn = ParseXmlNode(node, "z30-hour-last-return"),
                        IpLastReturn = ParseXmlNode(node, "z30-ip-last-return"),
                        NoLoans = ParseXmlNode(node, "z30-no-loans"),
                        Alpha = ParseXmlNode(node, "z30-alpha"),
                        Collection = ParseXmlNode(node, "z30-collection"),
                        CallNoType = ParseXmlNode(node, "z30-call-no-type"),
                        CallNo = ParseXmlNode(node, "z30-call-no"),
                        CallNoKey = ParseXmlNode(node, "z30-call-no-key"),
                        CallNo2Type = ParseXmlNode(node, "z30-call-no-2-type"),
                        CallNo2 = ParseXmlNode(node, "z30-call-no-2"),
                        CallNo2Key = ParseXmlNode(node, "z30-call-no-2-key"),
                        Description = ParseXmlNode(node, "z30-description"),
                        NoteOpac = ParseXmlNode(node, "z30-note-opac"),
                        NoteCirculation = ParseXmlNode(node, "z30-note-circulation"),
                        NoteInternal = ParseXmlNode(node, "z30-note-internal"),
                        OrderNumber = ParseXmlNode(node, "z30-note-order-number"),
                        InventoryNumber = ParseXmlNode(node, "z30-inventory-number"),
                        InventoryNumberDate = ParseXmlNode(node, "z30-inventory-number-date"),
                        LastShelfReportDate = ParseXmlNode(node, "z30-last-shelf-report-date"),
                        Price = ParseXmlNode(node, "z30-price"),
                        ShelfReportNumber = ParseXmlNode(node, "z30-shelf-report-number"),
                        OnShelfDate = ParseXmlNode(node, "z30-on-shelf-date"),
                        OnShelfSeq = ParseXmlNode(node, "z30-on-shelf-seq"),
                        DocNumber2 = ParseXmlNode(node, "z30-doc-num-2")
                    }
                };




                sections.Add(section);
            }

            return sections;
        }

        private static string ParseXmlNode(XmlNode node, string name)
        {
            if(node == null)
                return string.Empty;

            var parseNode = node.SelectSingleNode(name);

            if (parseNode == null)
                return string.Empty;

            return parseNode.InnerText;
        }




    }
}
