using System;
using System.Drawing;
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
            throw new NotImplementedException();
        }

        public Bitmap GenerateSpineLabel(LabelItem item)
        {
            throw new NotImplementedException();
        }
    }
}
