using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator.Interfaces
{
    public interface ISourceParser
    {
        //bool SourceLocation();

        LabelItem GenerateLabelItem(string location);
    }
}
