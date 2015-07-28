using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LabelPrinter.App.Extensions
{
    public static class WpfHelpers
    {
        public static BitmapImage ToBitmapImage(this Bitmap image)
        {
            using (var memory = new MemoryStream())
            {
                image.Save(memory, ImageFormat.Bmp);
                memory.Seek(0, SeekOrigin.Begin);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
