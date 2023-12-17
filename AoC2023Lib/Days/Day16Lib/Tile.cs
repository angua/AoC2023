namespace AoC2023Lib.Days.Day16Lib;

public class Tile
{
    public TileMirror Mirror {  get; set; }
    public bool IsEnergized { get; set; } = false;

    public List<Direction> IncomingBeams { get; set; } = new();
}
