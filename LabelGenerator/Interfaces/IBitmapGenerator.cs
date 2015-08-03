using System.Collections.Generic;
using System.Drawing;
using LabelGenerator.Objects.BitmapGenerator;
using LabelGenerator.Objects.LabelConfig;


namespace LabelGenerator.Interfaces
{
    public interface IBitmapGenerator
    {
        LabelBitmap GenerateLabel(Dictionary<string, string> label, LabelTemplate labelTemplate);
    }
}
