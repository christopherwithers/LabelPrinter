using System.Collections.Generic;

namespace LabelGenerator.Interfaces
{
    public interface ILabelManager
    {
        Dictionary<string, string> ParseLabelFromFile(string fileLocation);
    }
}
