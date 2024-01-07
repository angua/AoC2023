using System.Drawing;
using CommonWPF;

namespace AoC2023.Days.Day23;

internal class TileColorSelector : ITileColorSelector
{
    public Color GetColorForTile(Tile tile)
    {
        if (tile is not VisualTile visualTile)
        {
            return Color.FromArgb(0, 0, 0);
        }
        return visualTile.Ground switch
        {
            '.' => Color.FromArgb(141, 255, 40),
            '#' => Color.FromArgb(20, 109, 47),
            _ => Color.FromArgb(98, 242, 255),
        };
    }
}
