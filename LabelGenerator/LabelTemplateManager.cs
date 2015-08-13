using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.LabelConfig;
using Newtonsoft.Json;

namespace LabelGenerator
{
    public class LabelTemplateManager : ILabelTemplateManager
    {
        private List<LabelTemplate> _labels;

        public IFileManager FileManager { get; set; }

        public async Task<IEnumerable<LabelTemplate>> FetchAllLabelTemplates()
        {
            return await FileManager.ReadJsonFile($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json");
        }

        public bool SaveLabelTemplate(LabelTemplate label)
        {
            var updatedLabels = _labels.Where(n => n.Name != label.Name).ToList();

            updatedLabels.Add(label);

            return SaveAllLabelTemplates(updatedLabels);
        }

        public bool SaveAllLabelTemplates(IEnumerable<LabelTemplate> labels)
        {
            var success = true;
            try
            {
                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json", JsonConvert.SerializeObject(labels, Formatting.Indented));
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
