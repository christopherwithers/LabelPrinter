using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.LabelConfig;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace LabelGenerator
{
    public class FileSystemLabelRepository : ILabelRepository
    {
        private List<Label> _labels;
        
        public Label FetchLabel(string name)
        {
            if (_labels == null)
                LoadLabels();

            return _labels.FirstOrDefault(n => n.Name == name);
        }

        public IEnumerable<Label> FetchAllLabels()
        {
            if (_labels == null)
                LoadLabels();

            return _labels;
        }


        private void LoadLabels()
        {
            using (var r = new StreamReader(@"C:\Development\LabelPrinter\LabelGenerator\Config\labels.json"))
            {
                //string json = r.ReadToEnd();
                string currentLine;
                var json = new StringBuilder();
                while ((currentLine = r.ReadLine()) != null)
                    json.Append(currentLine.Trim());

                _labels = json.ToString().FromJson<List<Label>>();
            }
        }

        public bool SaveLabel(Label label)
        {
            var updatedLabels = _labels.Where(n => n.Name != label.Name).ToList();

          ////  if (updatedLabels.HasContent())
            //{
                updatedLabels.Add(label);
            //}

            return SaveAllLabels(updatedLabels);
        }

        public bool SaveAllLabels(IEnumerable<Label> labels)
        {
            var success = true;
            try
            {
                File.WriteAllText(@"C:\Development\LabelPrinter\LabelGenerator\Config\labels.json", JsonConvert.SerializeObject(labels, Newtonsoft.Json.Formatting.Indented));
            }
            catch (Exception)
            {
                success = false;
            }

            if (success)
                _labels = labels.ToList();

            return success;
        }

        public bool DeleteLabel(Label label)
        {
            throw new NotImplementedException();
        }
    }
}
