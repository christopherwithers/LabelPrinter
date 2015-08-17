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
using LabelGenerator;
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
        private readonly IFileManager _fileManager;
        private FileSystemWatcher _fsWatcher;
        DateTime _lastRead = DateTime.MinValue;

        public ObservableCollection<DisplayLabel> LabelImages { get; set; }

        public ViewLabels()
        {
            _labelTemplateManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();
            _labelManager = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelManager>();
            _bitmapGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<IBitmapGenerator>();
            _fileManager = FirstFloor.ModernUI.App.App.Container.GetInstance<IFileManager>();

            _labelLocation = new CommandLineArgs()["location"];
            LabelImages = new ObservableCollection<DisplayLabel>();
     
            InitializeComponent();

            DataContext = this;

            var configDirectory = $@"{AppDomain.CurrentDomain.BaseDirectory}\Config\";
            if (_fileManager.CheckDirectoryExists(configDirectory))
            {
                _fsWatcher = new FileSystemWatcher
                {
                    NotifyFilter = NotifyFilters.LastWrite,
                    Path = configDirectory,
                    Filter = "labels.json",
                    EnableRaisingEvents = true
                };
                _fsWatcher.Changed += FsWatcherOnChanged;

                GetImages();
            }
            else
            {
                ModernDialog.ShowMessage($"An error occurred. The '{configDirectory}' directory could not be found.", "Error", MessageBoxButton.OK, Window.GetWindow(this));

            }
            
        }

        

        private async void GetImages()
        {
            if (_labelLocation != null)
            {
                LabelImages.Clear();

                var labelItem = await _labelManager.ParseLabelFromFile(_labelLocation);

                    var templates = await _labelTemplateManager.FetchAllLabelTemplates();

                    if (templates.HasContent())
                    {
                        foreach (var template in templates)
                        {
                            var fullLabel = _bitmapGenerator.GenerateLabel(labelItem, template);

                            if (fullLabel != null)
                            {
                                LabelImages.Add(new DisplayLabel
                                {
                                    Bitmap = fullLabel.Bitmap,
                                    Template = fullLabel.Template
                                });
                            }

                        }
                    }
                    else
                    {
                        ModernDialog.ShowMessage("An error occurred. The label templates could not be loaded.","Error", MessageBoxButton.OK, Window.GetWindow(this));
                    }


                
            }
            else
            {
                ModernDialog.ShowMessage(@"The file path command line 'location' was not found.", "Error", MessageBoxButton.OK, Window.GetWindow(this));
                spPrintAll.Visibility = Visibility.Hidden;
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
                ModernDialog.ShowMessage(@"No printer is associated with this label. Please add one in the Edit Labels tab.", "Error", MessageBoxButton.OK, Window.GetWindow(this));
            }
            else
            {
                var selectedDisplayLabel  = LabelImages.SingleOrDefault(n => n.Template.TemplateName == printerString);

                if (selectedDisplayLabel == null)
                {
                    ModernDialog.ShowMessage(@"An error occurred. The requested label could not be printer.", "Error", MessageBoxButton.OK, Window.GetWindow(this));
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
                ModernDialog.ShowMessage(@"An error occurred. The labels could not be printed.", "Error", MessageBoxButton.OK, Window.GetWindow(this));
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
