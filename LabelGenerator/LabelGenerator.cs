using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.Extensions;
using LabelGenerator.Objects.SourceParser;
using Zen.Barcode;

namespace LabelGenerator
{
    public class LabelGenerator : ILabelGenerator
    {
        private readonly ISourceParser _sourceParser;

        public LabelGenerator(ISourceParser sourceParser)
        {
            _sourceParser = sourceParser;
        }

        public LabelItem ParseSourceItem(string location)
        {
            return _sourceParser.GenerateLabelItem(location);
        }

        public Bitmap GenerateFullLabel(LabelItem item)
        {
            var imageSize = new Point(500, 300);
            using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
            using (var graphics = Graphics.FromImage(labelImage))
            {
                var fontCollection = new PrivateFontCollection();

                fontCollection.AddFontFile(
                    @"E:\Development\LabelPrinter\LabelGenerator\bin\Debug\Fonts\ArialUnicodeMS.ttf");

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
            var imageSize = new Point(300, 200);
            using (var labelImage = new Bitmap(imageSize.X, imageSize.Y))
            using (var graphics = Graphics.FromImage(labelImage))
            {
                var fontCollection = new PrivateFontCollection();

                fontCollection.AddFontFile(AppDomain.CurrentDomain.BaseDirectory + @"\LabelGenerator\bin\Debug\Fonts\ArialUnicodeMS.ttf");

                var font20 = new Font(fontCollection.Families[0], 20f, FontStyle.Bold);

                graphics.Clear(Color.White);
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                var sb1 = new SolidBrush(Color.Black);

                var rect = new RectangleF(10, 10, imageSize.X-10, imageSize.Y-10);
                graphics.DrawString(item.Sections[0].Z30.CallNo, font20, sb1, rect);


                return new Bitmap(labelImage);
            }
        }
    }
}
