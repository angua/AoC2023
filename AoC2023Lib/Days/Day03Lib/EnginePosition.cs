namespace AoC2023Lib.Days.Day03Lib;

public class EnginePosition
{
    public char Symbol;

    public EnginePosition(char input)
    {
        Symbol = input;
    }

    public SymbolType Type { get; set; }

    public SchematicNumber? SchematicNumber { get; set; } = null;
}
