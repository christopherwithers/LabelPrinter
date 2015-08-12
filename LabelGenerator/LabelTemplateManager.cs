using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.LabelConfig;
using Newtonsoft.Json;

namespace LabelGenerator
{
    public class LabelTemplateManager : ILabelTemplateManager
    {
       // private readonly ISourceParser _sourceParser;
      //  private readonly ILabelRepository _labelRepository;
        
        private List<LabelTemplate> _labels;

        // private Dictionary<string, string> _sourceItemDict; 
        internal void LoadLabels(string location)
        {
            using (var r = new StreamReader(location))//new StreamReader(File.Open(location, FileMode.Open)))//
            {
                //string json = r.ReadToEnd();
                string currentLine;
                var json = new StringBuilder();
                while ((currentLine = r.ReadLine()) != null)
                    json.Append(currentLine.Trim());

                _labels = json.ToString().FromJson<List<LabelTemplate>>();
            }
        }

        public LabelTemplateManager(/*ILabelRepository labelRepository*/)
        {
          //  _sourceParser = sourceParser;
        //    _labelRepository = labelRepository;
        }

      //  public ISourceParser SourceParser { get; set; }

        public IEnumerable<LabelTemplate> FetchAllLabelTemplates()
        {
            //if (_labels == null)
                LoadLabels($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json");

            return _labels;
        }

        public bool SaveLabelTemplate(LabelTemplate label)
        {
            var updatedLabels = _labels.Where(n => n.Name != label.Name).ToList();

            ////  if (updatedLabels.HasContent())
            //{
            updatedLabels.Add(label);
            //}

            return SaveAllLabelTemplates(updatedLabels);
        }

        public bool SaveAllLabelTemplates(IEnumerable<LabelTemplate> labels)
        {
            var success = true;
            try
            {
                File.WriteAllText($@"{AppDomain.CurrentDomain.BaseDirectory}\Config\labels.json", JsonConvert.SerializeObject(labels, Formatting.Indented));
            }
            catch (Exception)
            {
                success = false;
            }

            if (success)
                _labels = labels.ToList();

            return success;
        }

        public bool DeleteLabel(LabelTemplate label)
        {
            throw new NotImplementedException();
        }

        /*
       public LabelItem ParseSourceItem(string location)
       {
           return _sourceParser.GenerateLabelItem(location);
       }


     public Dictionary<string, string> ParseSourceItemAsDictionary(string location)
     {
         return _sourceItemDict = _sourceParser.GenerateLabelItemDictionary(location);
     }


           public void LoadConfiguredLabels()
           {

           }

        private string ParseLabelText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.StartsWith("#") && text.EndsWith("#"))
            {
                string dictValue;
                var checkVal = text.Replace("#", "");
                if (_sourceItemDict.TryGetValue(checkVal.ToLower(), out dictValue))
                    return dictValue;
            }

            return text;
        }


                private static float ParseCoord(string text = "", string coord = "", Graphics graphics = null, Font font = null, float size = 0f)
                {
                    if (string.IsNullOrEmpty(coord))
                        return 0;

                    if (coord.ToLower() == "center")
                        return text.XCenter(graphics, font, size);

                    return Convert.ToSingle(coord);
                }

                public Bitmap GenerateFullLabel(LabelItem item)
                {
                    return GenerateLabelTest();
                    var imageSize = new Point(500, 300);
                    using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
                    using (var graphics = Graphics.FromImage(labelImage))
                    {
                        var fontCollection = new PrivateFontCollection();

                        fontCollection.AddFontFile(
                            @"C:\Development\LabelPrinter\LabelGenerator\bin\Debug\Fonts\ArialUnicodeMS.ttf");

                        var font10 = new Font(fontCollection.Families[0], 10f, FontStyle.Bold);
                        var font11 = new Font(fontCollection.Families[0], 11f, FontStyle.Bold);

                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        var sb1 = new SolidBrush(Color.Black);
                        var text = "University of Birmingham";
                        graphics.DrawString(text, font11, sb1, new PointF(text.XCenter(graphics, font11, imageSize.X), 10));
                        text = "Library Services";
                        graphics.DrawString(text, font10, sb1, new PointF(text.XCenter(graphics, font10, imageSize.X), 25));
                        text = item.Sections[0].Z30.SubLibrary;
                        graphics.DrawString(text, font10, sb1, new PointF(text.XCenter(graphics, font10, imageSize.X), 43));

                        graphics.DrawRectangle(new Pen(Color.Black), 10, 60, imageSize.X - 20, imageSize.Y - 70);

                        const int leftBorder = 20;
                        const int leftBorderOffset = 200;
                        var yContent = 70;

                        graphics.DrawString(item.Sections[0].Z13.Author, font10, sb1, leftBorder, yContent);
                        graphics.DrawString(item.Sections[0].Z13.Title, font10, sb1, leftBorder, yContent += 20);
                        graphics.DrawString(item.Sections[0].Z13.Year, font10, sb1, leftBorder, yContent += 20);
                        graphics.DrawString(item.Sections[0].Z13.IsbnIssn, font10, sb1, leftBorder, yContent += 20);

                        graphics.DrawString("Shelfmark:", font10, sb1, leftBorder, yContent += 90);
                        graphics.DrawString(item.Sections[0].Z30.CallNo, font11, sb1, leftBorder + leftBorderOffset, yContent);

                        graphics.DrawString("Barcode:", font10, sb1, leftBorder, yContent += 20);
                        graphics.DrawString(item.Sections[0].Z30.BarCode, font10, sb1, leftBorder + leftBorderOffset, yContent);

                        graphics.DrawString("System No:", font10, sb1, leftBorder, yContent += 20);
                        graphics.DrawString(item.Sections[0].Z13.DocNumber, font10, sb1, leftBorder + leftBorderOffset, yContent);

                        BarcodeDraw tst = new Code11BarcodeDraw(Code11Checksum.Instance);
                        var bc = tst.Draw(item.Sections[0].Z30.BarCode, 50);


                   //     if (bc != null)
                       //     graphics.DrawImage(bc, new PointF(bc.XCenter(imageSize.X), 150));

                        return new Bitmap(labelImage);
                    }
                }


                public Bitmap GenerateSpineLabel(LabelItem item)
                {
                    //return GenerateLabelTest();
                    var imageSize = new Point(300, 200);
                    using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
                    using (var graphics = Graphics.FromImage(labelImage))
                    {
                        var fontCollection = new PrivateFontCollection();

                        fontCollection.AddFontFile(@"C:\Development\LabelPrinter\LabelGenerator\bin\Debug\Fonts\ArialUnicodeMS.ttf");

                        var font20 = new Font(fontCollection.Families[0], 20f, FontStyle.Bold);

                        graphics.Clear(Color.White);
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                        var sb1 = new SolidBrush(Color.Black);

                        var rect = new RectangleF(10, 10, imageSize.X - 10, imageSize.Y - 10);
                        graphics.DrawString(item.Sections[0].Z30.CallNo, font20, sb1, rect);


                        return new Bitmap(labelImage);
                    }
                }*/
    }
}
