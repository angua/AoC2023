using System.Drawing;
using CommonWPF;

namespace AoC2023.Days.Day17;

internal class TileLineColorSelector : ITileLineColorSelector
{
    public Color GetColorForTileLine(TileLine tileLine)
    {
        return Color.FromArgb(0, 0, 0);
    }
}
