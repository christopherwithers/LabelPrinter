using System.Collections.Generic;
using LabelGenerator.Objects.LabelConfig;

namespace LabelGenerator.Interfaces
{
    public interface ILabelTemplateManager
    {
        IEnumerable<LabelTemplate> FetchAllLabelTemplates();

        bool SaveLabelTemplate(LabelTemplate label);
        bool SaveAllLabelTemplates(IEnumerable<LabelTemplate> labels);

        bool DeleteLabel(LabelTemplate label);
        // LabelItem ParseSourceItem(string location);
        // Dictionary<string, string> ParseSourceItemAsDictionary(string location); 


        // Bitmap GenerateFullLabel(LabelItem item);
        // Bitmap GenerateSpineLabel(LabelItem item);
    }
}
