using System.Drawing;
using CommonWPF;

namespace AoC2023.Days.Day17
{
    internal class TileColorSelector : ITileColorSelector
    {
        private ColourGradient _colours = new ColourGradient();

        public TileColorSelector()
        {
            _colours.AddToColourMappings(1, "#AA0000");
            _colours.AddToColourMappings(3, "#FF0000");
            _colours.AddToColourMappings(5, "#FFAA00");
            _colours.AddToColourMappings(7, "#FFFF00");
            _colours.AddToColourMappings(9, "#FFFFAA"); 
        }

        public Color GetColorForTile(Tile tile)
        {
            if (tile is not VisualTile visualTile)
            {
                return Color.FromArgb(0, 0, 0);
            }
            return ColorTranslator.FromHtml(_colours.GetColour(visualTile.HeatLoss));
        }
    }

}
