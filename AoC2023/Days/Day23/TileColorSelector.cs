using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CommonWPF;

namespace AoC2023.Days.Day23
{
    internal class TileColorSelector : ITileColorSelector
    {
        public Color GetColorForTile(Tile tile)
        {
            if (tile is not VisualTile visualTile)
            {
                return Color.FromRgb(0, 0, 0);
            }
            return visualTile.Ground switch
            {
                '.' => Color.FromRgb(141, 255, 40),
                '#' => Color.FromRgb(20, 109, 47),
                _ => Color.FromRgb(98, 242, 255),
            };
        }
    }
}
