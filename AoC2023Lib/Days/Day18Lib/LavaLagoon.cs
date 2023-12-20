using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day18Lib;

public class LavaLagoon
{
    public List<Instruction> Instructions { get; set; } = new();

    public List<Vector2> RealCorners { get; set; } = new();

    public long RealMinX { get; set; }
    public long RealMaxX { get; set; }
    public long RealMinY { get; set; }
    public long RealMaxY { get; set; }



    public Dictionary<Vector2, DigPosition> DigPlan { get; set; } = new();


    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            Instructions.Add(new Instruction(line));
        }
        foreach (var instruction in Instructions)
        {
            instruction.ConvertColorToRealInstruction();
        }

        RealCorners = GetRealCorners();
        RealMinX = (long)RealCorners.Min(p => p.X);
        RealMaxX = (long)RealCorners.Max(p => p.X);
        RealMinY = (long)RealCorners.Min(p => p.Y);
        RealMaxY = (long)RealCorners.Max(p => p.Y);


    }

    public long GetLavaVolume(bool useRealValues = false)
    {
        var edges = FillEdges(useRealValues);

        var inside = MathUtils.GetInside(edges);

        return edges.Count + inside.Count;
    }

    public long GetRealLavaVolume()
    {
        long doubleArea = 0;


        for (int i = 0; i < RealCorners.Count; i++)
        {
            var firstCorner = RealCorners[i];
            var secondCorner = (i + 1) < RealCorners.Count ? RealCorners[i + 1] : RealCorners[0];

            // Shoelace theorem incoming
            doubleArea += (long)firstCorner.X * (long)secondCorner.Y;
            doubleArea -= (long)firstCorner.Y * (long)secondCorner.X;
        }

        var area = doubleArea / 2;

        var edgeArea = Instructions.Sum(i => i.DistanceFromColor);

        // Pick's theorem
        // A + b / 2 + 1
        return area + edgeArea / 2 + 1;



       // return GetLavaVolume(true);
    }

    private List<Vector2> GetRealCorners()
    {
        var pos = new Vector2(0, 0);
        var corners = new List<Vector2>() { pos };

        foreach (var instruction in Instructions)
        {
            switch (instruction.DirFromColor)
            {
                case Direction.Right:
                    pos = new Vector2(pos.X + instruction.DistanceFromColor, pos.Y);
                    break;

                case Direction.Left:
                    pos = new Vector2(pos.X - instruction.DistanceFromColor, pos.Y);
                    break;

                case Direction.Down:
                    pos = new Vector2(pos.X, pos.Y + instruction.DistanceFromColor);
                    break;

                case Direction.Up:
                    pos = new Vector2(pos.X, pos.Y - instruction.DistanceFromColor);
                    break;
            }

            corners.Add(pos);
        }
        return corners;
    }

    public List<Vector2> FillEdges(bool useRealValues = false)
    {
        var edges = new List<Vector2>();
        var position = new Vector2(0, 0);

        foreach (var instruction in Instructions)
        {
            var dir = useRealValues ? instruction.DirFromColor : instruction.Dir;
            var distance = useRealValues ? instruction.DistanceFromColor : instruction.Distance;

            for (int i = 0; i < distance; i++)
            {
                position += GetVector(dir);
                edges.Add(position);
            }
        }
        return edges;
    }

    private Vector2 GetVector(Direction dir)
    {
        return dir switch
        {
            Direction.Right => new Vector2(1, 0),
            Direction.Left => new Vector2(-1, 0),
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1)
        };
    }

}

