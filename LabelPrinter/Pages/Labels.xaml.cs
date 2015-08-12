using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using Extensions;
using LabelGenerator.Interfaces;

namespace LabelPrinter.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Labels : UserControl
    {
        private readonly ILabelTemplateManager _labelGenerator;

        public IEnumerable<LabelGenerator.Objects.LabelConfig.LabelTemplate> _labels;


        public Labels()
        {
            InitializeComponent();

            _labelGenerator = FirstFloor.ModernUI.App.App.Container.GetInstance<ILabelTemplateManager>();

            if (_labelGenerator != null)
                GenerateFormItems();

        }

        private void GenerateFormItems()
        {
            if (_labels == null)
            {
                _labels = _labelGenerator.FetchAllLabelTemplates();
            }

            if (_labels.HasContent())
            {
                StackPanel stackPanel;
                foreach (var label in _labels)
                {
                    stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
                    stackPanel.Children.Add(new Label { Content = label.FriendlyName });



                    var comboBox = new ComboBox {Name = label.Name};

                    comboBox.SetBinding(
                       ItemsControl.ItemsSourceProperty,
                       new Binding { Source = GetInstalledPrinters() });

                    comboBox.SelectedValue = label.Printer;

                    comboBox.SetBinding(
                       Selector.SelectedItemProperty,
                       new Binding("Printer") { Source = label, Mode = BindingMode.TwoWay });

                    stackPanel.Children.Add(comboBox);

                    SpLabels.Children.Add(stackPanel);
                }

                stackPanel = new StackPanel { Orientation = Orientation.Horizontal };

                var saveButton = new Button { Name = "Save", Content = "Save" };
                saveButton.Click += SaveButtonOnClick;
                stackPanel.Children.Add(saveButton);

                SpLabels.Children.Add(stackPanel);
            }

        }

        private void SaveButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_labelGenerator.SaveAllLabelTemplates(_labels))
                _labels = _labelGenerator.FetchAllLabelTemplates();
            else
                MessageBox.Show("An error occurred, the printer settings could not be saved.");
        }

        private static ObservableCollection<string> GetInstalledPrinters()
        {
            var printServer = new LocalPrintServer();

            var printQueuesOnLocalServer = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            var printers = new ObservableCollection<string>(printQueuesOnLocalServer.Select(printer => printer.FullName).ToList());

            return printers;
        }
    }
}
