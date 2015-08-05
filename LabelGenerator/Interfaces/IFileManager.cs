using System.Threading.Tasks;

namespace LabelGenerator.Interfaces
{
    public interface IFileManager
    {
        Task<string> ReadFile(string location);
    }
}
