using System.Numerics;

namespace AoC2023Lib.Days.Day17Lib;

public class Move
{
    public Vector2 EndPosition { get; set; }
    public Vector2 Direction { get; set; }
    public int StraightCount { get; set; }
    public int Loss {  get; set; }
    public List<Vector2> Route {  get; set; }

}
