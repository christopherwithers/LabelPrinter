using System.Windows.Media.Imaging;
using LabelGenerator.Objects.BitmapGenerator;
using LabelPrinter.App.Extensions;

namespace LabelPrinter.App.Code.Models
{
    public class DisplayLabel : LabelBitmap
    {
        public BitmapImage DisplayImage => Bitmap?.ToBitmapImage() ?? new BitmapImage();
    }
}
