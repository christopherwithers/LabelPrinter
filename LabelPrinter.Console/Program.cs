using LabelGenerator;
using LabelGenerator.Interfaces;

namespace LabelPrinter.Console
{
    class Program
    {//http://stackoverflow.com/questions/15436334/printing-custom-paper-size-to-an-impact-printer-in-wpf?lq=1
        static void Main(string[] args)
        {
            ILabelGenerator sourceParser = new LabelGenerator.LabelGenerator(new XmlSourceParser());

            var item = sourceParser.ParseSourceItem(@"E:\Development\LabelPrinter\LabelGenerator\Files\Item.xml");

            if (item != null)
            {
                var fullLabel = sourceParser.GenerateFullLabel(item);

                var i = 1;
            }
        }
    }
}
