using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        private string _labelLocation;
        private readonly ILabelTemplateManager _labelTemplateManager;
        private readonly ILabelManager _labelManager;
        private readonly IBitmapGenerator _bitmapGenerator;
        private FileSystemWatcher _fsWatcher;
        DateTime _lastRead = DateTime.MinValue;
        // private Dictionary<string, string> _labelItem;

        public PrintLabels()
        {
            _labelTemplateManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();
            _labelManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelManager>();
            _bitmapGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<IBitmapGenerator>();

            _labelLocation = new CommandLineArgs()["location"];

         //   OpenFileDialogEx.FileSelected += OpenFileDialogExOnFileSelected;
            //  ErrorMessage(true);

            InitializeComponent();

            if (_labelLocation != null)
            {
                GenerateFormItems();

                if (_fsWatcher == null)
                {
                    try
                    {
                        _fsWatcher = new FileSystemWatcher
                        {
                            NotifyFilter = NotifyFilters.LastWrite,
                            Path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Config\",
                            Filter = "labels.json",
                            EnableRaisingEvents = true
                        };
                        _fsWatcher.Changed += FsWatcherOnChanged;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                

                MessageBox.Show(@"The file path command line 'location' was not found.");
              //  SPOpenFilePanel.Visibility = Visibility.Visible;
            }
        }

        private async void FsWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            // await GenerateFormItems();
            var lastWriteTime = File.GetLastWriteTime($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json");

            if (lastWriteTime != _lastRead)
            {
                GenerateFormItems();
                _lastRead = lastWriteTime;
            }

            
        }



        private async Task<bool> GenerateFormItems()
        {
            var labelFromFile = _labelManager?.ParseLabelFromFile(_labelLocation);

            if (labelFromFile != null)
            {


                Dictionary<string, string> _labelItem = await labelFromFile;

                if (!string.IsNullOrEmpty(_labelLocation))
                {
                    if (_labelItem != null)
                    {
                        /*if (SpLabels.Children.Count > 0)
                        {
                            SpLabels.Children.RemoveAt(SpLabels.Children.Count - 1);
                        }*/
                        foreach (var template in _labelTemplateManager.FetchAllLabelTemplates())
                        {
                            var fullLabel = _bitmapGenerator?.GenerateLabel(_labelItem, template);

                            if (fullLabel != null)
                            {
                                
                                   var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
                                stackPanel.Children.Add(new TextBlock
                                {
                                    Text = fullLabel.Template.FriendlyName,
                                    Style = (Style) FindResource("Heading2")
                                });

                                SpLabels.Children.Add(stackPanel);

                                stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
                                stackPanel.Children.Add(new Image {Source = fullLabel.Bitmap.ToBitmapImage(), Width = fullLabel.Bitmap.Width, Height = fullLabel.Bitmap.Height});
                                var printLabelButton = new ModernButton
                                {
                                    Name = fullLabel.Template.TemplateName,
                                    IconData = (Geometry) SpMain.Resources["PrintIcon"],
                                    EllipseDiameter = 64,
                                    EllipseStrokeThickness = 2,
                                    IconWidth = 42,
                                    IconHeight = 42
                                };
                                printLabelButton.Click +=
                                    (sender, args) =>
                                    {
                                        if (string.IsNullOrEmpty(template.Printer))
                                        {
                                            MessageBox.Show("No printer is associated with this label. Please add one in the Edit Labels tab.");
                                        }
                                        else
                                        {
                                            var printer = new LocalPrintServer().GetPrintQueue(template.Printer);

                                            var dialog = new PrintDialog
                                            {
                                                PrintQueue = printer
                                            };

                                            var vis = new DrawingVisual();
                                            var dc = vis.RenderOpen();
                                            var btmp = _bitmapGenerator.GenerateLabel(_labelItem, template);
                                            dc.DrawImage(btmp.Bitmap.ToBitmapImage(), new Rect { Width = btmp.Bitmap.Width, Height = btmp.Bitmap.Height });
                                            dc.Close();


                                            dialog.PrintVisual(vis, template.FriendlyName);

                                            dialog = null;
                                            vis = null;
                                            dc = null;
                                            btmp = null;
                                        }
                                    };
                                   
                                stackPanel.Children.Add(printLabelButton);
                                SpLabels.Children.Add(stackPanel);

                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }

    }
}
