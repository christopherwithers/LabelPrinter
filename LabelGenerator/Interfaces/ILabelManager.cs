using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabelGenerator.Interfaces
{
    public interface ILabelManager
    {
        Task<Dictionary<string, string>> ParseLabelFromFile(string fileLocation);
    }
}
