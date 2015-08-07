using System.Drawing;
using LabelGenerator.Objects.LabelConfig;

namespace LabelGenerator.Objects.BitmapGenerator
{
    public class LabelBitmap
    {
        public LabelTemplate Template { get; set; }
        public Bitmap Bitmap { get; set; }
    }
}
