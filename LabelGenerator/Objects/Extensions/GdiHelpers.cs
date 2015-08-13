using System.Drawing;

namespace LabelGenerator.Objects.Extensions
{
    public static class GdiHelpers
    {
        public static float XCenter(this string text, Graphics g, Font font, float containerWidth)
        {
            var s = g.MeasureString(text, font);
             
            return (containerWidth - s.Width) / 2;
        }

        public static float YCenter(this string text, Graphics g, Font font, float containerHeight)
        {
            var s = g.MeasureString(text, font);

            return (containerHeight - s.Height) / 2;
        }

        /*public static float XCenter(this Image image, float containerWidth)
        {
            return (containerWidth - image.Width) / 2;
        }*/
    }
}
