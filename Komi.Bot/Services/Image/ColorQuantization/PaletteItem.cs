using System.Drawing;

namespace Komi.Bot.Services.Image.ColorQuantization
{
    public struct PaletteItem
    {
        public Color Color { get; set; }

        public int Weight { get; set; }
    }
}