using System.Numerics;

namespace AoC2023Lib.Days.Day17Lib;

public class PositionData
{
    // <direction, moves in this direction>
    public Dictionary<Vector2, List<Move>> Moves = new();
}
