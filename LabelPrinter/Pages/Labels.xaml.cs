using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using Extensions;
using FirstFloor.ModernUI.Windows.Controls;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.SourceParser;
using LabelPrinter.App.Extensions;

namespace LabelPrinter.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Labels : UserControl
    {
        private readonly ILabelGenerator _labelGenerator;

        private IEnumerable<LabelGenerator.Objects.LabelConfig.Label> _labels;


        public Labels()
        {
            InitializeComponent();

            _labelGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelGenerator>();

            if (_labelGenerator != null)
                GenerateFormItems();
            
        }

        private void GenerateFormItems()
        {
            if (_labels == null)
            {
                _labels = _labelGenerator.FetchAllLabels();
            }

            if (_labels.HasContent())
            {
                foreach (var label in _labels)
                {
                    var stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    stackPanel.Children.Add(new Label { Content = label.FriendlyName });
                    stackPanel.Children.Add(new ModernButton { IconData = (Geometry) SpMain.Resources["PrintIcon"] });
                    stackPanel.Children.Add(new ModernButton { IconData = (Geometry)SpMain.Resources["PrintIcon"] });

                    var comboBox = new ComboBox();
                    GetInstalledPrinters().ForEach(i => comboBox.Items.Add(i));

                   // if (!string.IsNullOrEmpty(label.Printer))
                        comboBox.SelectedItem = label.Printer;
                    stackPanel.Children.Add(comboBox);

                    var printConfigButton = new ModernButton {IconData = (Geometry) SpMain.Resources["PrintIcon"], Name = label.Name};
                    printConfigButton.Click += PrintConfigButtonOnClick;
                    stackPanel.Children.Add(printConfigButton);
                    SpLabels.Children.Add(stackPanel);
                }
            }

        }

        private static List<string> GetInstalledPrinters()
        {
            var printServer = new LocalPrintServer();

            var printQueuesOnLocalServer = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            var printers = printQueuesOnLocalServer.Select(printer => printer.FullName).ToList();

            return printers;
        }

      /*  private class tstDialog : PrintDialog
        {
            public tstDialog()
            {
                this.Controls
               // ((ToolStripButton)((ToolStrip)this.Controls[1]).Items[0]).Enabled = false;
            }

            
        }*/

        private void PrintConfigButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var source = (ModernButton) sender;

            if (source != null)
            {

                var dlg = new PrintDialog();

                var result = dlg.ShowDialog();

                if (result.HasValue && result.Value)
                {
                    var label = _labels.FirstOrDefault(n => n.Name == source.Name);

                    if (label != null)
                    {
                        label.Printer = dlg.PrintQueue.FullName;

                        if (!_labelGenerator.SaveLabel(label))
                            MessageBox.Show("Couldn't save!");
                    }
                }
            }
        }
    }
}
