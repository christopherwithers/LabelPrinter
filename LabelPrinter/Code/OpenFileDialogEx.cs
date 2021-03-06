﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace LabelPrinter.App.Code
{
    public class OpenFileDialogEx
    {
        public static event EventHandler<FileSelectedEventArgs> FileSelected;

        public static readonly DependencyProperty FilterProperty =
          DependencyProperty.RegisterAttached("Filter",
            typeof(string),
            typeof(OpenFileDialogEx),
            new PropertyMetadata("All documents (.*)|*.*", (d, e) => AttachFileDialog((TextBox)d, e)));

        public static string GetFilter(UIElement element)
        {
            return (string)element.GetValue(FilterProperty);
        }

        public static void SetFilter(UIElement element, string value)
        {
            element.SetValue(FilterProperty, value);
        }

        private static void AttachFileDialog(TextBox textBox, DependencyPropertyChangedEventArgs args)
        {
            var parent = (Panel)textBox.Parent;

            parent.Loaded += delegate
            {

                var button = (Button)parent.Children.Cast<object>().FirstOrDefault(x => x is Button);

                var filter = (string)args.NewValue;

                if (button != null)
                    button.Click += (s, e) =>
                    {
                        var dlg = new OpenFileDialog { Filter = filter };

                        var result = dlg.ShowDialog();

                        if (result == true)
                        {
                            textBox.Text = dlg.FileName;
                            FileSelected?.Invoke(null, new FileSelectedEventArgs(dlg.FileName));
                        }

                    };
            };
        }
    }

    public class FileSelectedEventArgs : EventArgs
    {
        public string FileName { get; internal set; }

        public FileSelectedEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
