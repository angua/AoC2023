using System.Collections.Generic;
using System.Numerics;

namespace AoC2023.Days.Day13;

internal class GridPosition
{
    private KeyValuePair<Vector2, byte> _element;

    public GridPosition(KeyValuePair<Vector2, byte> element)
    {
        _element = element;
    }

    public int PositionX => (int)_element.Key.X;
    public int PositionY => (int)_element.Key.Y;

    public byte GroundType => _element.Value;

    public bool IsSmudged { get; set; } = false;
}
