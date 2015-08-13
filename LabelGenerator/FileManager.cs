using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.LabelConfig;
using NLog;

namespace LabelGenerator
{   //Doe we need to go to the lengths in the link RE: pass in stream, so we can test this?
    //http://stackoverflow.com/questions/9540472/how-to-unit-test-a-class-which-needs-a-specific-file-to-be-present
    public class FileManager : IFileManager
    {
        private ILogger Log { get; set; }

        public FileManager()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        

        public virtual async Task<string> ReadXMLFile(string location)
        {
            try
            {
                if (!CheckFileExists(location))
                    throw new FileNotFoundException();
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
                Log.Fatal(ex);
            }

            return string.Empty;
        }

        public async Task<List<LabelTemplate>> ReadJsonFile(string location)
        {
            try
            {
                if (!CheckFileExists(location))
                    throw new FileNotFoundException();

                using (var r = new StreamReader(location))//new StreamReader(File.Open(location, FileMode.Open)))//
                {
                    //string json = r.ReadToEnd();
                    string currentLine;
                    var json = new StringBuilder();
                    while ((currentLine = await r.ReadLineAsync()) != null)
                        json.Append(currentLine.Trim());

                    return json.ToString().FromJson<List<LabelTemplate>>();
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }

            return null;
        }

        public bool CheckFileExists(string location)
        {
            try
            {
                return File.Exists(location);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }
            return false;
        }

        public bool CheckDirectoryExists(string location)
        {
            try
            {
                return Directory.Exists(location);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
            }
            return false;
        }
    }
}
