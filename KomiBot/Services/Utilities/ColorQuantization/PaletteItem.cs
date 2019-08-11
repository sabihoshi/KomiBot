using System.Drawing;

namespace KomiBot.Services.Utilities.ColorQuantization
{
    public struct PaletteItem
    {
        public Color Color { get; set; }

        public int Weight { get; set; }
    }
}
