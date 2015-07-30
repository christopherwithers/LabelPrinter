using System.Collections.Generic;
using System.Drawing;
using LabelGenerator.Objects.LabelConfig;
using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator.Interfaces
{
    public interface ILabelGenerator
    {
        IEnumerable<Label> FetchAllLabels();

        bool SaveLabel(Label label);
        bool SaveAllLabels(IEnumerable<Label> labels);

        LabelItem ParseSourceItem(string location);
        Dictionary<string, string> ParseSourceItemAsDictionary(string location); 
        Bitmap GenerateFullLabel(LabelItem item);
        Bitmap GenerateSpineLabel(LabelItem item);
    }
}
