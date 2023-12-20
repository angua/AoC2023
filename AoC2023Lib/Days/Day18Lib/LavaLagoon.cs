using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day18Lib;

public class LavaLagoon
{
    public List<Instruction> Instructions = new();

    public Dictionary<Vector2, DigPosition> DigPlan { get; set; } = new();


    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            Instructions.Add(new Instruction(line));
        }
    }

    public int GetLavaVolume()
    {
        var edges = FillEdges();

        var inside = MathUtils.GetInside(edges);

        return edges.Count + inside.Count;
    }


    public List<Vector2> FillEdges()
    {
        var edges = new List<Vector2>();
        var position = new Vector2(0, 0);

        foreach (var instruction in Instructions)
        {
            var dir = GetVector(instruction.Direction);

            for (int i = 0; i < instruction.NumberOfHoles; i++)
            {
                position += dir;
                DigPlan[position] = new DigPosition()
                {
                    Type = DiggingType.Edge,
                    EdgeColor = instruction.Color
                };
                edges.Add(position);
            }
        }
        return edges;
    }

    private Vector2 GetVector(string input)
    {
        return input switch
        {
            "R" => new Vector2(1, 0),
            "L" => new Vector2(-1, 0),
            "U" => new Vector2(0, -1),
            "D" => new Vector2(0, 1)
        };
    }

}
