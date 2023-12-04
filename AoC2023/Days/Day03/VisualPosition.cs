using System.Collections.Generic;
using System.Numerics;
using CommonWPF;

namespace AoC2023.Days.Day03;

public class VisualPosition : ViewModelBase
{
    public VisualPosition(KeyValuePair<Vector2, char> pos)
    {
        PositionX = (int)pos.Key.X;
        PositionY = (int)pos.Key.Y;
        Character = pos.Value;
    }

    public int PositionX { get; }
    public int PositionY { get; }
    public char Character { get; }

    public bool IsSchematicNumber
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }

    public bool IsGear
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }
}