using System.Numerics;

namespace AoC2023Lib.Days.Day11Lib;

public class SpacePosition
{
    public SpaceType Type { get; set; }

    public Vector2 Position { get; set; }

    public SpacePosition(char input)
    {
        Type = input switch
        {
            '#' => SpaceType.Galaxy,
            '.' => SpaceType.Empty,
        };

    }
}
