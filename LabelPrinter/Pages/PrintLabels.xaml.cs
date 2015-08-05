using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FirstFloor.ModernUI.Windows.Controls;
using LabelGenerator.Interfaces;
using LabelPrinter.App.Extensions;

namespace LabelPrinter.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class PrintLabels : UserControl
    {
        private readonly string _labelLocation;
        private readonly ILabelTemplateManager _labelTemplateManager;
        private readonly ILabelManager _labelManager;
        private readonly IBitmapGenerator _bitmapGenerator;

        private Dictionary<string, string> _labelItem;

        public PrintLabels()
        {
            _labelTemplateManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();
            _labelManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelManager>();
            _bitmapGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<IBitmapGenerator>();

            _labelLocation = new CommandLineArgs()["location"];

            if (_labelLocation == null)
                ErrorMessage(true);

            InitializeComponent();

            GenerateFormItems();
        }


        private async void GenerateFormItems()
        {
            var labelFromFile = _labelManager?.ParseLabelFromFile(_labelLocation);

            if (labelFromFile != null)
            {
                _labelItem = await labelFromFile;

                if (!string.IsNullOrEmpty(_labelLocation))
                {
                    if (_labelItem != null)
                    {
                        foreach (var template in _labelTemplateManager.FetchAllLabelTemplates())
                        {
                            var fullLabel = _bitmapGenerator?.GenerateLabel(_labelItem, template);

                            if (fullLabel != null)
                            {
                                var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
                                stackPanel.Children.Add(new TextBlock
                                {
                                    Text = "Label Name",
                                    Style = (Style) FindResource("Heading2")
                                });
                                SpLabels.Children.Add(stackPanel);

                                stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
                                stackPanel.Children.Add(new Image {Source = fullLabel.Bitmap.ToBitmapImage()});
                                var printLabelButton = new ModernButton
                                {
                                    Name = "Labelname",
                                    IconData = (Geometry) SpMain.Resources["PrintIcon"]
                                };
                                printLabelButton.Click += PrintLabelButtonOnClick;
                                stackPanel.Children.Add(printLabelButton);
                                SpLabels.Children.Add(stackPanel);

                            }
                        }
                    }
                }
            }
        }

        private static void PrintLabelButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            MessageBox.Show(((ModernButton) sender).Name);
        }

        private void ErrorMessage(bool isError, string errorMessage = "")
        {
         /*   if (isError)
            {
                SpLabels.Children.Add(new Label {Content = "Error!"});
            }*/
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
    }
}
