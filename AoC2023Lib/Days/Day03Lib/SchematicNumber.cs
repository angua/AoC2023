﻿using System.Numerics;

namespace AoC2023Lib.Days.Day03Lib;

public class SchematicNumber
{
    public Dictionary<Vector2, char> Positions { get; set; } = new();

    public Vector2 StartPosition
    {
        get
        {
            var minX = Positions.Min(p => p.Key.X);
            return Positions.First(p => p.Key.X == minX).Key;
        }
    }

    public int Number
    {
        get
        {
            var numStr = string.Join("", Positions.Values);
            return int.Parse(numStr);
        }
    }

}
