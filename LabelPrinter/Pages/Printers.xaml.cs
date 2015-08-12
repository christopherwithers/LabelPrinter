using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.SourceParser;
using LabelPrinter.App.Extensions;

namespace LabelPrinter.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Printers : UserControl
    {
        private readonly string _labelLocation;
        private readonly ILabelTemplateManager _labelTemplateManager;
        private readonly LabelItem _labelItem;

        public BitmapImage GetImgLargeLabel
        {
            get { return GetLargeLabel(); }
        }

        public BitmapImage GetImgSpineLabel
        {
            get { return GetSpineLabel(); }
        }

        public Printers()
        {
            _labelTemplateManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();
            
            var args = Environment.GetCommandLineArgs();

            _labelLocation = args[1];

          //  if (_labelTemplateManager != null)
              //  _labelItem = _labelTemplateManager.ParseSourceItem(_labelLocation);

            InitializeComponent();
        }


        private void GenerateFormItems()
        {
            
        }

        private BitmapImage GetLargeLabel()
        {
            /*if (!string.IsNullOrEmpty(_labelLocation))
            {
                if (_labelItem != null)
                {
                    var fullLabel = _labelTemplateManager.GenerateFullLabel(_labelItem);

                    if (fullLabel != null)
                    {
                        return fullLabel.ToBitmapImage();
                    }
                }
            }*/

            return new BitmapImage();
        }

        private BitmapImage GetSpineLabel()
        {
        /*    if (!string.IsNullOrEmpty(_labelLocation))
            {
                if (_labelItem != null)
                {
                    var fullLabel = _labelTemplateManager.GenerateSpineLabel(_labelItem);

                    if (fullLabel != null)
                    {
                        return fullLabel.ToBitmapImage();
                    }
                }
            }*/

            return new BitmapImage();
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            
            /*var printerSettings = new PrinterSettings();
            var labelPaperSize = new PaperSize { RawKind = (int)PaperKind.A6, Height = 148, Width = 105 };
            printerSettings.DefaultPageSettings.PaperSize = labelPaperSize;
            var labelPaperSource = new PaperSource { RawKind = (int)PaperSourceKind.Manual };
            printerSettings.DefaultPageSettings.PaperSource = labelPaperSource;
            if (printerSettings.CanDuplex)
            {
                printerSettings.Duplex = Duplex.Default;
            }

            var pd = new PrintDocument
            {
                DefaultPageSettings = {Margins = new Margins(0, 0, 0, 0)},
                OriginAtMargins = false, 
                PrinterSettings = printerSettings
            };

            var pdialog = new PrintDialog();

            if (pdialog.ShowDialog() == true)
            {
                var vis = new DrawingVisual();
                var dc = vis.RenderOpen();
                var btmp = GetLargeLabel();
                    dc.DrawImage(btmp, new Rect { Width = btmp.Width, Height = btmp.Height });
                    dc.Close();


                   // System.Printing.ValidationResult result = pdialog.PrintQueue.MergeAndValidatePrintTicket(pdialog.PrintQueue.UserPrintTicket, pdialog.PrintTicket);

                    pdialog.PrintVisual(vis, "my print job");
            }*/

//
  //          DoPreview("sdfdsf");


            //############################
            //Working:
          /*  PrintDialog dlg = new PrintDialog();
            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
                ImgLargeLabel.Measure(new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight));
                ImgLargeLabel.Arrange(new Rect(new Point(0, 0), ImgLargeLabel.DesiredSize));

                dlg.PrintVisual(ImgLargeLabel, "Print a Large Image");
            }*/

            var dlg = new PrintDialog();

            var result = dlg.ShowDialog();

            if (result.HasValue && result.Value)
            {
             //   ImgLargeLabel.Measure(new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight));
               // ImgLargeLabel.Arrange(new Rect(new Point(0, 0), ImgLargeLabel.DesiredSize));

              //  dlg.PrintVisual(ImgLargeLabel, "Print a Large Image");

             //   var someVar = dlg.PrintQueue.FullName;

                MessageBox.Show(dlg.PrintQueue.FullName);
            }

           // printDialog.PrintQueue = new PrintQueue(new PrintServer(), "PrinterName");
            
            //printDialog.PrintDocument(document, "PrintDocument");
            //######################################
        }


        //##################

        private string _previewWindowXaml =
    @"<Window
        xmlns                 ='http://schemas.microsoft.com/netfx/2007/xaml/presentation'
        xmlns:x               ='http://schemas.microsoft.com/winfx/2006/xaml'
        Title                 ='Print Preview - @@TITLE'
        Height                ='200'
        Width                 ='300'
        WindowStartupLocation ='CenterOwner'>
        <DocumentViewer Name='dv1'/>

     </Window>";

        internal void DoPreview(string title)
        {
            string fileName = System.IO.Path.GetRandomFileName();
            FlowDocumentScrollViewer visual = new FlowDocumentScrollViewer();// (FlowDocumentScrollViewer)(_parent.FindName("fdsv1"));
            try
            {
                // write the XPS document
                using (var doc = new XpsDocument(fileName, FileAccess.ReadWrite))
                {
                    var writer = XpsDocument.CreateXpsDocumentWriter(doc);
                    writer.Write(visual);
                }

                // Read the XPS document into a dynamically generated
                // preview Window 
                using (XpsDocument doc = new XpsDocument(fileName, FileAccess.Read))
                {
                    FixedDocumentSequence fds = doc.GetFixedDocumentSequence();



                    string s = _previewWindowXaml;
                    s = s.Replace("@@TITLE", title.Replace("'", "&apos;"));

                    using (var reader = new System.Xml.XmlTextReader(new StringReader(s)))
                    {
                        Window preview = System.Windows.Markup.XamlReader.Load(reader) as Window;

                       // DocumentViewer dv1 = LogicalTreeHelper.FindLogicalNode(preview, "dv1") as DocumentViewer;
                      //  dv1.Document = fds as IDocumentPaginatorSource;

                        //dv1

                        preview.ShowDialog();
                    }
                }
            }
            finally
            {
               /* if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {
                    }
                }*/
            }
        } 

        //##################









    }
}
