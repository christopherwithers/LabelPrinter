using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using ServiceStack.Text;

namespace Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Serialises an object to Json using the ServiceStack Json serialiser.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return JsonSerializer.SerializeToString(obj);
        }

     /*   public static int ToPixel(this int mm)
        {
            return mm;
            float dx, dy;

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dx = graphics.DpiX;
                dy = graphics.DpiY;
            }

            return (int) ((mm* 203) / 25.4);
        }

        public static float ToPixel(this float mm)
        {
            float dx, dy;

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dx = graphics.DpiX;
                dy = graphics.DpiY;
            }

            return (float)((mm * dx) / 25.4);
        }

        public static double ToPixel(this double mm)
        {
            float dx, dy;

            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dx = graphics.DpiX;
                dy = graphics.DpiY;
            }

            return (double)((mm * dx) / 25.4);
        }*/

        /// <summary>
        /// Converts Json object to the specified 'T' class object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string obj)
        {
            return JsonSerializer.DeserializeFromString<T>(obj);
        }


        /// <summary>
        /// Checks if a list is not null and contains objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool HasContent<T>(this IList<T> collection)
        {
            return collection != null && collection.Any();
        }

        public static bool HasContent<T>(this IEnumerable<T> collection)
        {
            //TODO: Will this cause performance issues???
            return collection != null && collection.Skip(0).Any();
        }

        public static bool HasContent<T, T1>(this IDictionary<T, T1> collection)
        {
            return collection != null && collection.Any();
        }


        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
    }
}
