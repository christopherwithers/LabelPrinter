using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LabelGenerator.Interfaces;
using NLog;

namespace LabelGenerator
{   //Doe we need to go to the lengths in the link RE: pass in stream, so we can test this?
    //http://stackoverflow.com/questions/9540472/how-to-unit-test-a-class-which-needs-a-specific-file-to-be-present
    public class FileManager : IFileManager
    {
        public FileManager()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        public ILogger Log { get; set; }

        public virtual async Task<string> ReadFile(string location)
        {
            try
            {
                //using (var r = new StreamReader(File.Open(location, FileMode.Open)))
                using (var r = new StreamReader(location))
                {
                    string currentLine;
                    var xmlDoc = new StringBuilder();
                    while ((currentLine = await r.ReadLineAsync()) != null)
                        xmlDoc.Append(currentLine.Trim());

                    return xmlDoc.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return string.Empty;
        }
    }
}
