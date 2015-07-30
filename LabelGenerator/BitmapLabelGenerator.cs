using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Extensions;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.Extensions;
using LabelGenerator.Objects.LabelConfig;
using LabelGenerator.Objects.SourceParser;
using Zen.Barcode;
using Type = LabelGenerator.Objects.LabelConfig.Type;

namespace LabelGenerator
{
    public class BitmapLabelGenerator : ILabelGenerator
    {
        private readonly ISourceParser _sourceParser;
        private readonly ILabelRepository _labelRepository;
 
        public BitmapLabelGenerator(ISourceParser sourceParser, ILabelRepository labelRepository)
        {
            _sourceParser = sourceParser;
            _labelRepository = labelRepository;
        }

        public IEnumerable<Label> FetchAllLabels()
        {
            return _labelRepository.FetchAllLabels();
        }

        public bool SaveLabel(Label label)
        {
            return _labelRepository.SaveLabel(label);
        }

        public bool SaveAllLabels(IEnumerable<Label> labels)
        {
            return _labelRepository.SaveAllLabels(labels);
        }

        public LabelItem ParseSourceItem(string location)
        {
            return _sourceParser.GenerateLabelItem(location);
        }

        public Dictionary<string, string> ParseSourceItemAsDictionary(string location)
        {
            throw new NotImplementedException();
        }

        public void LoadConfiguredLabels()
        {

        }

        private Bitmap GenerateLabelTest()
        {
          //  LoadJson();
            var Label = new Label { Name = "FullLabel", Width = 500, Height = 300 };

            Label.Content.Add(new DisplayLine { Type = Type.String, X = "10", Y = "10", Text = "University of Birmingham", Font = "ArialUnicode-11f" });

            Label.Fonts.Add(new FontConfig { Custom = true, Location = "ArialUnicodeMS.ttf", FontFamily = "Arial Unicode MS", Name = "ArialUnicode-10f", Size = 10f, Style = FontStyle.Bold });
            Label.Fonts.Add(new FontConfig { Custom = true, Location = "ArialUnicodeMS.ttf", FontFamily = "Arial Unicode MS", Name = "ArialUnicode-11f", Size = 11f, Style = FontStyle.Bold });

            

           // var collectionss = new Collection<Label>();
           // collectionss.Add(Label);
           // collectionss.Add(Label);


           // var test = collectionss.ToJson();
            //var blah = test.FromJson<Label>();


            if (Label != null)
            {
                var imageSize = new Point(Label.Width, Label.Height);
                using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
                using (var graphics = Graphics.FromImage(labelImage))
                {
                    var fontCollection = new PrivateFontCollection();

                    var customFonts = Label.Fonts.Where(n => n.Custom).Select(n => n.Location).Distinct();

                    if (customFonts.HasContent())
                    {
                        foreach (var font in customFonts)
                        {
                            fontCollection.AddFontFile(@"C:\Development\LabelPrinter\LabelGenerator\bin\Debug\Fonts\" + font);
                        }
                    }

                    var fontDict = new Dictionary<string, Font>();

                    foreach (var font in Label.Fonts)
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
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    graphics.PageUnit = GraphicsUnit.Millimeter;
                    var sb1 = new SolidBrush(Color.Black);

                    if (Label.Content.HasContent())
                    {
                        foreach (var line in Label.Content)
                        {
                            switch (line.Type)
                            {
                                case Type.String:
                                    graphics.DrawString(line.Text, fontDict[line.Font], sb1,
                                        new Point(Convert.ToInt32(line.X), Convert.ToInt32(line.Y)));
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    return new Bitmap(labelImage);
                    //  fontCollection.Families.Select(n => n.Name == )
                    /* fontCollection.AddFontFile(
                        @"C:\Development\LabelPrinter\LabelGenerator\bin\Debug\Fonts\ArialUnicodeMS.ttf");

                    var fontDict = new Dictionary<TextFont, Font>
                    {
                        {TextFont.Font10, new Font(fontCollection.Families[0], 10f, FontStyle.Bold)},
                        {TextFont.Font11, new Font(fontCollection.Families[0], 11f, FontStyle.Bold)}
                    };


                    var font10 = new Font(fontCollection.Families[0], 10f, FontStyle.Bold);
                    var font11 = new Font(fontCollection.Families[0], 11f, FontStyle.Bold);


                    graphics.Clear(Color.White);
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    var sb1 = new SolidBrush(Color.Black);*/

                    /*var tst = (item) =>
                    {
                        if (item.ToLower() == "center")
                        {
                            return 10;
                        }
                        else
                        {
                            return 20;
                        }
                    }*/

                    /*
                                        if (Label.Content.HasContent())
                                        {
                                            foreach (var line in Label.Content)
                                            {
                                                switch (line.Type)
                                                {
                                                    case Type.String:
                                                        graphics.DrawString(line.Text, font11, sb1, new PointF(text.XCenter(graphics, font11, imageSize.X), 10));
                                                        break;

                                                    default:
                                                        break;
                                                }
                                            }
                                        }*/

                }
            }
            return null;
        }

        public Bitmap GenerateFullLabel(LabelItem item)
        {
           // return GenerateLabelTest();
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


                if (bc != null)
                    graphics.DrawImage(bc, new PointF(bc.XCenter(imageSize.X), 150));

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
        }
    }
}
