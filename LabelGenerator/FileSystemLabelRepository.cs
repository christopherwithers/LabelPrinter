using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.LabelConfig;
using Newtonsoft.Json;

namespace LabelGenerator
{
    public class FileSystemLabelRepository : ILabelRepository
    {
        private List<LabelTemplate> _labels;
        
        public LabelTemplate FetchLabel(string name)
        {
            if (_labels == null)
                LoadLabels();

            return _labels?.FirstOrDefault(n => n.Name == name);
        }

        public IEnumerable<LabelTemplate> FetchAllLabels()
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

                _labels = json.ToString().FromJson<List<LabelTemplate>>();
            }
        }

        public bool SaveLabel(LabelTemplate label)
        {
            var updatedLabels = _labels.Where(n => n.Name != label.Name).ToList();

          ////  if (updatedLabels.HasContent())
            //{
                updatedLabels.Add(label);
            //}

            return SaveAllLabels(updatedLabels);
        }

        public bool SaveAllLabels(IEnumerable<LabelTemplate> labels)
        {
            var success = true;
            try
            {
                File.WriteAllText(@"C:\Development\LabelPrinter\LabelGenerator\Config\labels.json", JsonConvert.SerializeObject(labels, Formatting.Indented));
            }
            catch (Exception)
            {
                success = false;
            }

            if (success)
                _labels = labels.ToList();

            return success;
        }

        public bool DeleteLabel(LabelTemplate label)
        {
            throw new NotImplementedException();
        }
    }
}
