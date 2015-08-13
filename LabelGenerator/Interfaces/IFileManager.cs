using System.Collections.Generic;
using System.Threading.Tasks;
using LabelGenerator.Objects.LabelConfig;

namespace LabelGenerator.Interfaces
{
    public interface IFileManager
    {
        Task<string> ReadXMLFile(string location);
        Task<List<LabelTemplate>> ReadJsonFile(string location);
        bool CheckFileExists(string location);
        bool CheckDirectoryExists(string location);
    }
}
