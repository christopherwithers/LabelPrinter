﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using LabelGenerator;
using LabelGenerator.Objects;
using Moq;
using NUnit.Framework;

namespace LabelPrinter.Tests
{    /*
    * ################################################
    * ############# NAMING CONVENTION ################
    * ################################################
    * # MethodName_StateUnderTest_ ExpectedBehaviour #
    * ################################################
    */
#if DEBUG
    [TestFixture]
    public class LabelManagerTests
    {
        private LabelManager _labelManager;
       // private LabelManager _labelManagerMock;
        private Mock<FileManager> _fileManager;

        

        private const string ValidLocation = "ValidLocation";
        private const string InvalidLocation = "InvalidLocation";

        private const string XmlString = "<?xml version=\"1.0\"?> <printout> <form-name>item-copy-label</form-name> <form-language>ENG</form-language> <form-format>00</form-format> <subject>Item Copy Label</subject> <section-01> <form-date>27/07/2015</form-date> <bib-info>Jones, Robin.: The art of C programming / Robin Jones, Ian Stewart..</bib-info> <z13-doc-number>374182</z13-doc-number> <z13-year>1987</z13-year> <z13-open-date>11/06/2015</z13-open-date> <z13-update-date>11/06/2015</z13-update-date> <z13-call-no-key></z13-call-no-key> <z13-call-no-code>090</z13-call-no-code> <z13-call-no>QA76.73.C15 J66 1987</z13-call-no> <z13-author-code>1001</z13-author-code> <z13-author>Jones, Robin.</z13-author> <z13-title-code>24514</z13-title-code> <z13-title>The art of C programming /</z13-title> <z13-imprint-code>260</z13-imprint-code> <z13-imprint>New York ; London : Springer-Verlag, 1987.</z13-imprint> <z13-isbn-issn-code>020</z13-isbn-issn-code> <z13-isbn-issn>0387963928</z13-isbn-issn> <z13-upd-time-stamp>201506132334339</z13-upd-time-stamp> <z13u-doc-number>000374182</z13u-doc-number> <z13u-user-defined-1-code></z13u-user-defined-1-code> <z13u-user-defined-1>QA 76.73.C15## Jones, Robin. : The art of C programming / Robin Jones, Ian Stewart.. [000374182</z13u-user-defined-1> <z13u-user-defined-2-code></z13u-user-defined-2-code> <z13u-user-defined-2>ISBN: 0387963928## Jones, Robin.: The art of C programming / Robin Jones, Ian Stewart.## New York ; London : Springer-Verlag, 1987. [000374182]</z13u-user-defined-2> <z13u-user-defined-3-code></z13u-user-defined-3-code> <z13u-user-defined-3></z13u-user-defined-3> <z13u-user-defined-4-code></z13u-user-defined-4-code> <z13u-user-defined-4></z13u-user-defined-4> <z13u-user-defined-5-code></z13u-user-defined-5-code> <z13u-user-defined-5></z13u-user-defined-5> <z13u-user-defined-6-code></z13u-user-defined-6-code> <z13u-user-defined-6></z13u-user-defined-6> <z13u-user-defined-7-code></z13u-user-defined-7-code> <z13u-user-defined-7></z13u-user-defined-7> <z13u-user-defined-8-code></z13u-user-defined-8-code> <z13u-user-defined-8></z13u-user-defined-8> <z13u-user-defined-9-code></z13u-user-defined-9-code> <z13u-user-defined-9></z13u-user-defined-9> <z13u-user-defined-10-code></z13u-user-defined-10-code> <z13u-user-defined-10></z13u-user-defined-10> <z13u-user-defined-11-code></z13u-user-defined-11-code> <z13u-user-defined-11></z13u-user-defined-11> <z13u-user-defined-12-code></z13u-user-defined-12-code> <z13u-user-defined-12></z13u-user-defined-12> <z13u-user-defined-13-code></z13u-user-defined-13-code> <z13u-user-defined-13></z13u-user-defined-13> <z13u-user-defined-14-code></z13u-user-defined-14-code> <z13u-user-defined-14></z13u-user-defined-14> <z13u-user-defined-15-code></z13u-user-defined-15-code> <z13u-user-defined-15></z13u-user-defined-15> <z30-doc-number>374182</z30-doc-number> <z30-item-sequence> 3.0</z30-item-sequence> <z30-barcode>16693469</z30-barcode> <z30-sub-library>Main Library</z30-sub-library> <z30-material>Unknown</z30-material> <z30-item-status>Long loan</z30-item-status> <z30-open-date>26/03/1995</z30-open-date> <z30-update-date>01/10/2013</z30-update-date> <z30-cataloger>CONV</z30-cataloger> <z30-date-last-return></z30-date-last-return> <z30-hour-last-return></z30-hour-last-return> <z30-ip-last-return></z30-ip-last-return> <z30-no-loans>000</z30-no-loans> <z30-alpha>L</z30-alpha> <z30-collection></z30-collection> <z30-call-no-type>0</z30-call-no-type> <z30-call-no>QA 76.73.C15 J</z30-call-no> <z30-call-no-key>0 qa!76 73 c15 j 0</z30-call-no-key> <z30-call-no-2-type>0</z30-call-no-2-type> <z30-call-no-2>QA76.73.C15 J66</z30-call-no-2> <z30-call-no-2-key>0 qa!76 73 c15 j66</z30-call-no-2-key> <z30-description></z30-description> <z30-note-opac></z30-note-opac> <z30-note-circulation></z30-note-circulation> <z30-note-internal></z30-note-internal> <z30-order-number></z30-order-number> <z30-inventory-number></z30-inventory-number> <z30-inventory-number-date></z30-inventory-number-date> <z30-last-shelf-report-date>00000000</z30-last-shelf-report-date> <z30-price></z30-price> <z30-shelf-report-number>366246</z30-shelf-report-number> <z30-on-shelf-date>00000000</z30-on-shelf-date> <z30-on-shelf-seq>000000</z30-on-shelf-seq> <z30-doc-number-2>000000000</z30-doc-number-2> <z30-schedule-sequence-2>00000</z30-schedule-sequence-2> <z30-copy-sequence-2>00000</z30-copy-sequence-2> <z30-vendor-code></z30-vendor-code> <z30-invoice-number></z30-invoice-number> <z30-line-number>00000</z30-line-number> <z30-pages></z30-pages> <z30-issue-date></z30-issue-date> <z30-expected-arrival-date></z30-expected-arrival-date> <z30-arrival-date></z30-arrival-date> <z30-item-statistic></z30-item-statistic> <z30-item-process-status>Not in Process</z30-item-process-status> <z30-copy-id></z30-copy-id> <z30-hol-doc-number>000000000</z30-hol-doc-number> <z30-temp-location>No</z30-temp-location> <z30-enumeration-a></z30-enumeration-a> <z30-enumeration-b></z30-enumeration-b> <z30-enumeration-c></z30-enumeration-c> <z30-enumeration-d></z30-enumeration-d> <z30-enumeration-e></z30-enumeration-e> <z30-enumeration-f></z30-enumeration-f> <z30-enumeration-g></z30-enumeration-g> <z30-enumeration-h></z30-enumeration-h> <z30-chronological-i></z30-chronological-i> <z30-chronological-j></z30-chronological-j> <z30-chronological-k></z30-chronological-k> <z30-chronological-l></z30-chronological-l> <z30-chronological-m></z30-chronological-m> <z30-supp-index-o></z30-supp-index-o> <z30-85x-type></z30-85x-type> <z30-depository-id></z30-depository-id> <z30-linking-number>000000000</z30-linking-number> <z30-gap-indicator></z30-gap-indicator> <z30-maintenance-count>000</z30-maintenance-count> <z30-process-status-date></z30-process-status-date> <z30-upd-time-stamp>201506260048592</z30-upd-time-stamp> <z30-ip-last-return-v6></z30-ip-last-return-v6> <tab-label-01></tab-label-01> <tab-label-02></tab-label-02> <tab-label-03></tab-label-03> <tab-label-04></tab-label-04> <tab-label-05></tab-label-05> <tab-label-06></tab-label-06> <tab-label-07></tab-label-07> <tab-label-08></tab-label-08> <tab-label-09></tab-label-09> <tab-label-10></tab-label-10> <call-no-piece-01>QA</call-no-piece-01> <call-no-piece-02>76.73</call-no-piece-02> <call-no-piece-03>.C15</call-no-piece-03> <call-no-piece-04>J</call-no-piece-04> <call-no-piece-05></call-no-piece-05> <call-no-piece-06></call-no-piece-06> <call-no-piece-07></call-no-piece-07> <call-no-piece-08></call-no-piece-08> <call-no-piece-09></call-no-piece-09> <call-no-piece-10></call-no-piece-10> <call-no-piece-2-01>QA</call-no-piece-2-01> <call-no-piece-2-02>76.73</call-no-piece-2-02> <call-no-piece-2-03>.C15</call-no-piece-2-03> <call-no-piece-2-04>J66</call-no-piece-2-04> <call-no-piece-2-05></call-no-piece-2-05> <call-no-piece-2-06></call-no-piece-2-06> <call-no-piece-2-07></call-no-piece-2-07> <call-no-piece-2-08></call-no-piece-2-08> <call-no-piece-2-09></call-no-piece-2-09> <call-no-piece-2-10></call-no-piece-2-10> <item-desc-piece-01></item-desc-piece-01> <item-desc-piece-02></item-desc-piece-02> <item-desc-piece-03></item-desc-piece-03> <item-desc-piece-04></item-desc-piece-04> <item-desc-piece-05></item-desc-piece-05> <item-desc-piece-06></item-desc-piece-06> <item-desc-piece-07></item-desc-piece-07> <item-desc-piece-08></item-desc-piece-08> <item-desc-piece-09></item-desc-piece-09> <item-desc-piece-10></item-desc-piece-10> </section-01> </printout>";

        [SetUp]
        public void SetUp()
        {
            _fileManager = new Mock<FileManager>();
          //  _labelManagerMock = new Mock<LabelManager>(_fileManager.Object).Object;
            _labelManager = new LabelManager(_fileManager.Object);
        }

#region SourceLocation Tests

        [Test]
        public async Task SourceLocation_ValidLocationValidXMLFormat_ReturnsTrue()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync(XmlString);

            var result = await _labelManager.SourceLocation(ValidLocation);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task SourceLocation_ValidLocationInvalidFile_ReturnsFalse()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync("InvalidXML");

            var result = await _labelManager.SourceLocation(ValidLocation);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SourceLocation_InvalidLocation_ReturnsFalse()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var result = await _labelManager.SourceLocation(InvalidLocation);

            Assert.IsFalse(result);
        }

#endregion

#region ConvertXmlDocumentToXDocument Tests
        [Test]
        public void ConvertXmlDocumentToXDocument_InvalidXmlDocument_ReturnsNull()
        {
          
            var result = _labelManager.ConvertXmlDocumentToXDocument(new XmlDocument());

            Assert.IsNull(result);
        }

        [Test]
        public void ConvertXmlDocumentToXDocument_ValidXmlDocument_ReturnsXDocument()
        {
            var validXmlDoc = new XmlDocument();
            validXmlDoc.LoadXml(XmlString);

            var result = _labelManager.ConvertXmlDocumentToXDocument(validXmlDoc);

            Assert.IsInstanceOf<XDocument>(result);
        }
#endregion

#region ParseLabelFromFile Tests
        [Test]
        public async Task ParseLabelFromFile_InvalidLocation_ReturnsNull()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync(string.Empty);

            var result = await _labelManager.ParseLabelFromFile(InvalidLocation);

            Assert.IsNull(result);
        }

        [Test]
        public async Task ParseLabelFromFile_ValidLocationInvalidXMLGenerated_ReturnsNull()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync(string.Empty);

            _labelManager.Document = new XmlDocument();

            var result = await _labelManager.ParseLabelFromFile(ValidLocation);

            Assert.IsNull(result);
        }

        [Test]
        public async Task ParseLabelFromFile_ValidLocation_ReturnsDictionary()
        {
            _fileManager.Setup(n => n.ReadXMLFile(It.IsAny<string>())).ReturnsAsync(XmlString);

            var result = await _labelManager.ParseLabelFromFile($"{ValidLocation}.xml");

            Assert.IsInstanceOf<Dictionary<string, string>>(result);
        }

#endregion

        [TearDown]
        public void TearDown()
        {
            _fileManager = null;
            _labelManager = null;
        }
    }
#endif
}
