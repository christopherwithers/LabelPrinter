using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.SourceParser;

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
            var labelImage = new Bitmap(400, 400);
            var graphics = Graphics.FromImage(labelImage);
            var font = new Font(FontFamily.GenericSansSerif, 14f);

            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            
            var sb1 = new SolidBrush(Color.Black);//Color.FromArgb(70, Color.WhiteSmoke)

            graphics.DrawString(item.FormName, font, sb1, 20, 20);


            return labelImage;
        }

        public Bitmap GenerateSpineLabel(LabelItem item)
        {
            throw new NotImplementedException();
        }
    }
}
