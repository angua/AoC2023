using AoC2023Lib.Days.Day23Lib;

namespace AoC2023.Days.Day23;

public class VisualPosition
{
    private Tile _tile;

    public int PositionX => (int)_tile.Position.X;
    public int PositionY => (int)_tile.Position.Y;

    public char Ground => _tile.Ground;

    public VisualPosition(Tile tile)
    {
        _tile = tile;
    }
}
