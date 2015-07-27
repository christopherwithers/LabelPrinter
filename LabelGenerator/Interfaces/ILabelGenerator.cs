using System.Drawing;
using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator.Interfaces
{
    public interface ILabelGenerator
    {
        LabelItem ParseSourceItem(string location);

        Bitmap GenerateFullLabel(LabelItem item);
        Bitmap GenerateSpineLabel(LabelItem item);
    }
}
