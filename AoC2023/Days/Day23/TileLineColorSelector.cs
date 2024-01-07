﻿using System.Drawing;
using CommonWPF;

namespace AoC2023.Days.Day23;

internal class TileLineColorSelector : ITileLineColorSelector
{
    public Color GetColorForTileLine(TileLine tileLine)
    {
        return Color.FromArgb(255, 0, 0);
    }
}
