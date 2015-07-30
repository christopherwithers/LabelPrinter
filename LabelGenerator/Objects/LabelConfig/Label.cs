using System.Collections.ObjectModel;
using System.Drawing;

namespace LabelGenerator.Objects.LabelConfig
{
    public enum Type { String, Rectangle }

    public class Label
    {
        public Label()
        {
            Content = new Collection<DisplayLine>();
            Fonts = new Collection<FontConfig>();
        }

        public string Name { get; set; }
        public string FriendlyName { get; set; }

        public string Printer { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Collection<DisplayLine> Content { get; set; }
        public Collection<FontConfig> Fonts { get; set; } 
    }

    public class FontConfig
    {
        public string Name { get; set; }
        public string FontFamily { get; set; }
        public bool Custom { get; set; }
        public string Location { get; set; }
        public float Size { get; set; }
        public FontStyle Style { get; set; }
    }

    public class DisplayLine
    {
        public Type Type { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public string Font { get; set; }
        
    }
}
