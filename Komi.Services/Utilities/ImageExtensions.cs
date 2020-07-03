using System.Drawing;
using System.IO;
using ColorThiefDotNet;
using Color = System.Drawing.Color;

namespace Komi.Bot.Services.Utilities
{
    public static class ImageExtensions
    {
        public static Bitmap ToBitmap(this byte[] bytes)
        {
            using var stream = new MemoryStream(bytes);
            return (Bitmap)System.Drawing.Image.FromStream(stream);
        }

        public static Color ToColor(this QuantizedColor color)
        {
            var c = color.Color;
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }
}