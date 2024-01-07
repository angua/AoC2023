using System.Drawing;
using CommonWPF;

namespace AoC2023.Days.Day17
{
    internal class TileColorSelector : ITileColorSelector
    {
        private ColourGradient _colours = new ColourGradient();

        public TileColorSelector()
        {
            _colours.AddToColourMappings(1, "#FF0000");
            _colours.AddToColourMappings(5, "#FFFF00");
            _colours.AddToColourMappings(9, "#00FF00");
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
