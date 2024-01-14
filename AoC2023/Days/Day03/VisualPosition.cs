using System.Collections.Generic;
using System.Numerics;
using AoC2023Lib.Days.Day03Lib;
using CommonWPF;

namespace AoC2023.Days.Day03;

public class VisualPosition : PositionedText 
{
    private EnginePosition _enginePosition;
    public VisualPosition(KeyValuePair<Vector2, EnginePosition> pos)
    {
        _enginePosition = pos.Value;

        PositionX = (int)pos.Key.X;
        PositionY = (int)pos.Key.Y;
        // replace '.' with empty space
        Text = _enginePosition.Symbol == '.' ? " " : _enginePosition.Symbol.ToString();
    }

    public SymbolType Type => _enginePosition.Type;
}