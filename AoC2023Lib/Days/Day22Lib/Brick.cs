using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day22Lib;

internal class Brick
{
    public int Id { get; set; }

    public Vector3 StartPos1 { get; set; }
    public Vector3 StartPos2 { get; set; }

    public List<Vector3> StartPositions { get; set; } = new();

    public List<Vector3> Endpositions { get; set; } = new();

    public HashSet<int> BricksBelow { get; set; } = new();
    public HashSet<int> BricksAbove { get; set; } = new();


    public Brick(string line)
    {
        // 1,0,1~1,2,1
        var parts = line.Split('~');

        var firstPosParts = parts[0].Split(',');
        StartPos1 = new Vector3(int.Parse(firstPosParts[0]), int.Parse(firstPosParts[1]), int.Parse(firstPosParts[2]));

        var secondPosParts = parts[1].Split(',');
        StartPos2 = new Vector3(int.Parse(secondPosParts[0]), int.Parse(secondPosParts[1]), int.Parse(secondPosParts[2]));

        for (int x = (int)StartPos1.X; x <= (int)StartPos2.X; x++)
        {
            for (int y = (int)StartPos1.Y; y <= (int)StartPos2.Y; y++)
            {
                for (int z = (int)StartPos1.Z; z <= (int)StartPos2.Z; z++)
                {
                    StartPositions.Add(new Vector3(x, y, z));
                }
            }
        }


    }
}
