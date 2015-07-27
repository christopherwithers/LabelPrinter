using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator.Interfaces
{
    public interface ISourceParser
    {
        bool SourceLocation(string location);

        LabelItem GenerateLabelItem();
    }
}
