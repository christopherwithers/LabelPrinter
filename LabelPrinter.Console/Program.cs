using LabelGenerator;
using LabelGenerator.Interfaces;

namespace LabelPrinter.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            ILabelGenerator sourceParser = new LabelGenerator.LabelGenerator(new XmlSourceParser());

            var item = sourceParser.ParseSourceItem(@"C:\Development\LabelPrinterApp\LabelGenerator\Files\Item.xml");

            if (item != null)
            {
                var fullLabel = sourceParser.GenerateFullLabel(item);
            }
        }
    }
}
