using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.BitmapGenerator;
using LabelGenerator.Objects.Extensions;
using LabelGenerator.Objects.LabelConfig;
using NLog;
using Type = LabelGenerator.Objects.LabelConfig.Type;

namespace LabelGenerator
{
    public class BitmapGenerator : IBitmapGenerator
    {
        private ILogger Log { get; set; }

        public BitmapGenerator()
        {
            Log = LogManager.GetCurrentClassLogger();
        }

        public IFileManager FileManager { get; set; }

        public LabelBitmap GenerateLabel(Dictionary<string, string> label, LabelTemplate labelTemplate)
        {
            try
            {
                return GenerateBitmapFromLabelTemplate(labelTemplate, label);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex);
                return null;
            } 
        }

        private LabelBitmap GenerateBitmapFromLabelTemplate(LabelTemplate label, Dictionary<string, string> labelContent)
        {
            if (label != null)
            {
                var imageSize = new Point(label.Width, label.Height);
                using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
                using (var graphics = Graphics.FromImage(labelImage))
                {
                    labelImage.SetResolution(300, 300);

                    var fontCollection = new PrivateFontCollection();

                    var customFonts = label.Fonts.Where(n => n.Custom).Select(n => n.Location).Distinct();

                    if (customFonts.HasContent())
                    {
                        foreach (var font in customFonts)
                        {
                            var file = $@"{AppDomain.CurrentDomain.BaseDirectory}\Fonts\{font}";

                            try
                            {
                                fontCollection.AddFontFile(file);
                            }
                            catch (Exception ex)
                            {
                                Log.Fatal($"The custom font '{file}' could not be found.");
                                return null;
                            }

                        }
                    }

                    var fontDict = new Dictionary<string, Font>();

                    foreach (var font in label.Fonts)
                    {
                        if (font.Custom)
                            fontDict.Add(font.Name, new Font(fontCollection.Families.Single(n => n.Name == font.FontFamily), font.Size, font.Style));
                        else
                            fontDict.Add(font.Name, new Font(font.FontFamily, font.Size, font.Style));
                    }

                    graphics.Clear(Color.White);
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    var sb1 = new SolidBrush(Color.Black);

                    if (label.Content.HasContent())
                    {
                        foreach (var line in label.Content)
                        {
                            switch (line.Type)
                            {
                                case Type.String:
                                    var text = ParseLabelText(line.Text, labelContent);
                                    graphics.DrawString(text, fontDict[line.Font], sb1,
                                        new PointF(ParseCoord(text, line.X, graphics, fontDict[line.Font], imageSize.X),
                                                   ParseCoord(text, line.Y, graphics, fontDict[line.Font], imageSize.Y)));
                                    break;
                                case Type.Rectangle:
                                    graphics.DrawRectangle(new Pen(Color.Black), ParseCoord(coord: line.X), ParseCoord(coord: line.Y), line.Width, line.Height);
                                    break;
                            }
                        }
                    }

                    return new LabelBitmap { Bitmap = new Bitmap(labelImage), Template = label };

                }
            }
            return null;
        }


        private string ParseLabelText(string text, Dictionary<string, string> labelContent)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.StartsWith("#") && text.EndsWith("#"))
            {
                string dictValue;
                var checkVal = text.Replace("#", "");
                if (labelContent.TryGetValue(checkVal.ToLower(), out dictValue))
                    return dictValue;
            }

            return text;
        }

        private static float ParseCoord(string text = "", string coord = "", Graphics graphics = null, Font font = null, float size = 0f)
        {
            if (string.IsNullOrEmpty(coord))
                return 0;

            if (coord.ToLower() == "centerx")
                return text.XCenter(graphics, font, size);

            if (coord.ToLower() == "centery")
                return text.YCenter(graphics, font, size);
            
            return Convert.ToSingle(coord);
        }
    }
}
