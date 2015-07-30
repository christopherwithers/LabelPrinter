using System.Collections.Generic;
using LabelGenerator.Objects.SourceParser;

namespace LabelGenerator.Interfaces
{
    public interface ISourceParser
    {
        //bool SourceLocation();

        LabelItem GenerateLabelItem(string location);
        Dictionary<string, string> GenerateLabelItemDictionary(string location);
    }
}
