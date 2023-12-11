using System.Numerics;

namespace AoC2023Lib.Days.Day10Lib;

public class Pipe
{
    public Vector2 Position { get; set; }
    public PipeOrientation Orientation { get; set; }

    public List<Vector2> Directions { get; set; }

    public Dictionary<Vector2, Pipe> Neighbors { get; set; } = new();

    public int? DistanceFomStart { get; set; } = null;


    public static List<Vector2> GetNeighborDirections(Pipe pipe)
    {
        return pipe.Orientation switch
        {
            PipeOrientation.None => new List<Vector2>(),

            PipeOrientation.NorthSouth => new List<Vector2>()
            {
                new Vector2(0, -1),
                new Vector2(0 ,1)
            },

            PipeOrientation.WestEast => new List<Vector2>()
            {
                new Vector2(-1, 0),
                new Vector2(1, 0)
            },

            PipeOrientation.NorthWest => new List<Vector2>()
            {
                new Vector2(0, -1),
                new Vector2(-1 ,0)
            },

            PipeOrientation.NorthEast => new List<Vector2>()
            {
                new Vector2(0, -1),
                new Vector2(1 ,0)
            },

            PipeOrientation.SouthWest => new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(-1 ,0)
            },

            PipeOrientation.SouthEast => new List<Vector2>()
            {
                new Vector2(0, 1),
                new Vector2(1 ,0)
            },

            PipeOrientation.Start => new List<Vector2>(),
        };
    }

}
