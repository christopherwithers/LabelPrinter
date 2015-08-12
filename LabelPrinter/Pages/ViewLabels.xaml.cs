using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Extensions;
using FirstFloor.ModernUI.Windows.Controls;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.BitmapGenerator;
using LabelGenerator.Objects.LabelConfig;
using LabelPrinter.App.Code.Models;
using LabelPrinter.App.Extensions;

namespace LabelPrinter.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class ViewLabels : UserControl
    {
        private string _labelLocation;
        private readonly ILabelTemplateManager _labelTemplateManager;
        private readonly ILabelManager _labelManager;
        private readonly IBitmapGenerator _bitmapGenerator;
        private FileSystemWatcher _fsWatcher;
        DateTime _lastRead = DateTime.MinValue;

        public ObservableCollection<DisplayLabel> LabelImages { get; set; }

        public ViewLabels()
        {
            _labelTemplateManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();
            _labelManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelManager>();
            _bitmapGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<IBitmapGenerator>();

            _labelLocation = new CommandLineArgs()["location"];
            LabelImages = new ObservableCollection<DisplayLabel>();
            //   OpenFileDialogEx.FileSelected += OpenFileDialogExOnFileSelected;
            //  ErrorMessage(true);


            // GetImages();
            InitializeComponent();
            DataContext = this;


            _fsWatcher = new FileSystemWatcher
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Path = $@"{AppDomain.CurrentDomain.BaseDirectory}\Config\",
                Filter = "labels.json",
                EnableRaisingEvents = true
            };
            _fsWatcher.Changed += FsWatcherOnChanged;

            GetImages();


        }

        private async void GetImages()
        {
            if (_labelLocation != null)
            {
                LabelImages.Clear();

                var labelFromFile = _labelManager?.ParseLabelFromFile(_labelLocation);

                if (labelFromFile != null)
                {
                    
                    Dictionary<string, string> _labelItem = await labelFromFile;

                    foreach (var template in _labelTemplateManager.FetchAllLabelTemplates())
                    {
                        var fullLabel = _bitmapGenerator?.GenerateLabel(_labelItem, template);

                        if(fullLabel != null)
                            LabelImages.Add(new DisplayLabel { Bitmap = fullLabel.Bitmap, Template = fullLabel.Template});
                    }


                }
            }
            else
            {
                MessageBox.Show(@"The file path command line 'location' was not found.");
                //  SPOpenFilePanel.Visibility = Visibility.Visible;
            }
        }

        private void FsWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            // await GenerateFormItems();
            var lastWriteTime = File.GetLastWriteTime($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json");

            if (lastWriteTime != _lastRead)
            {
                Application.Current.Dispatcher.InvokeAsync(GetImages);
                _lastRead = lastWriteTime;
            }

            
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var printerString = ((Button) sender).Tag.ToString();

            if (string.IsNullOrEmpty(printerString))
            {
                MessageBox.Show("No printer is associated with this label. Please add one in the Edit Labels tab.");
            }
            else
            {
                var selectedDisplayLabel  = LabelImages.SingleOrDefault(n => n.Template.TemplateName == printerString);

                if (selectedDisplayLabel == null)
                {
                    MessageBox.Show("An error occurred. The requested label could not be printer.");
                    return;
                }

                var printer = new LocalPrintServer().GetPrintQueue(selectedDisplayLabel.Template.Printer);

                var dialog = new PrintDialog
                {
                    PrintQueue = printer
                };

                var vis = new DrawingVisual();
                var dc = vis.RenderOpen();
                dc.DrawImage(selectedDisplayLabel.Bitmap.ToBitmapImage(), new Rect { Width = selectedDisplayLabel.Bitmap.Width, Height = selectedDisplayLabel.Bitmap.Height });
                dc.Close();


                dialog.PrintVisual(vis, selectedDisplayLabel.Template.FriendlyName);

            }
        }

        private void PrintAll_OnClick(object sender, RoutedEventArgs e)
        {
            if (!LabelImages.HasContent())
            {
                MessageBox.Show("An error occurred. The labels could not be printed.");
            }
            else
            {
                foreach (var label in LabelImages)
                {
                    var printer = new LocalPrintServer().GetPrintQueue(label.Template.Printer);

                    var dialog = new PrintDialog
                    {
                        PrintQueue = printer
                    };

                    var vis = new DrawingVisual();
                    var dc = vis.RenderOpen();
                    dc.DrawImage(label.Bitmap.ToBitmapImage(), new Rect { Width = label.Bitmap.Width, Height = label.Bitmap.Height });
                    dc.Close();


                    dialog.PrintVisual(vis, label.Template.FriendlyName);
                }
            }
        }
    }
}
