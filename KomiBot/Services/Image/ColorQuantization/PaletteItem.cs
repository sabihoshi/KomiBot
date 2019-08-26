using System.Drawing;

namespace KomiBot.Services.Image.ColorQuantization
{
    public struct PaletteItem
    {
        public Color Color { get; set; }

        public int Weight { get; set; }
    }
}