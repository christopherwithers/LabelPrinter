using System;
using System.IO;
using LabelGenerator.Interfaces;
using LabelGenerator.Objects.SourceParser;

namespace LabelPrinter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ISourceParser sourceParser = new XmlSourceParser();

            if (sourceParser.SourceLocation(@"C:\Development\LabelGenerator\Files\Item.xml"))
            {
                sourceParser.GenerateLabelItem();
            }
        }
    }
}
