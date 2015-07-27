using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LabelGenerator;
using LabelGenerator.Interfaces;


namespace FirstFloor.ModernUI.App.Pages
{
    /// <summary>
    /// Interaction logic for Introduction.xaml
    /// </summary>
    public partial class Introduction : UserControl
    {

        public BitmapImage ImageAdd
        {
            get { return getImg(); }
        }

        public Introduction()
        {
            InitializeComponent();
        }

        private BitmapImage getImg()
        {
            ILabelGenerator sourceParser = new LabelGenerator.LabelGenerator(new XmlSourceParser());

            var item = sourceParser.ParseSourceItem(@"E:\Development\LabelPrinter\LabelGenerator\Files\Item.xml");

            if (item != null)
            {
                var fullLabel = sourceParser.GenerateFullLabel(item);

                if (fullLabel != null)
                {
                    var c = new ImageSourceConverter();
                    return (BitmapImage)c.ConvertFrom(fullLabel);
                }
            }

            return new BitmapImage();
        }
    }
}
