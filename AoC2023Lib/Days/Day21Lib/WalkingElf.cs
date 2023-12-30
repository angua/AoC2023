using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day21Lib;

public class WalkingElf
{
    private Dictionary<Vector2, char> _grid = new();

    private int _countX;
    private int _countY;

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var line = fileData.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var pos = new Vector2(x, y);
                var ground = line[x];
                _grid[pos] = ground;
            }
        }
        _countX = (int)_grid.Max(p => p.Key.X) + 1;
        _countY = (int)_grid.Max(p => p.Key.Y) + 1;
    }

    public long GetPlots64Steps()
    {
        return GetPlotCountForSteps(64);
    }

    public long GetPlots26MSteps()
    {
        var countAtFullGridWalk = new Dictionary<long, long>();

        var start = _grid.First(p => p.Value == 'S').Key;
        var positions = new HashSet<Vector2>();
        positions.Add(start);

        var steps = 0;

        for (int i = 1; i <= 65; i++)
        {
            steps++;
            var nextPositions = WalkToNewPositions(positions);
            positions = nextPositions;
        }
        countAtFullGridWalk[steps] = positions.Count;

        for (int i = 0; i < 3; i++)
        {
            for (int s  = 0; s < _countX; s++)
            {
                steps++;
                var nextPositions = WalkToNewPositions(positions);
                positions = nextPositions;
            }
            countAtFullGridWalk[steps] = positions.Count;
        }

        // solve quadratic equation
        // y = a x^2 + bx + c

        // a = ( (y3 - y2)(x3 - x1) - (y3 - y1)(x3 - x2) ) / ( (x3 ^2 - x2 ^2)(x3 - x1) - (x3 ^2 - x1 ^2)(x3 - x2) )
        // b = ((y3 - y2) - a (x3 ^2 - x2 ^2)) / (x3 - x2)
        // c = y3 - a * x3 ^ 2 - b x3

        var x1 = (double)countAtFullGridWalk.Skip(1).First().Key;
        var y1 = (double)countAtFullGridWalk.Skip(1).First().Value;

        var x2 = (double)countAtFullGridWalk.Skip(2).First().Key;
        var y2 = (double)countAtFullGridWalk.Skip(2).First().Value;
                 
        var x3 = (double)countAtFullGridWalk.Skip(3).First().Key;
        var y3 = (double)countAtFullGridWalk.Skip(3).First().Value;

        var a = ((y3 - y2) * (x3 - x1) - (y3 - y1) * (x3 - x2)) /
                    ((Math.Pow(x3, 2.0) - Math.Pow(x2, 2.0)) * (x3 - x1) - (Math.Pow(x3, 2.0) - Math.Pow(x1, 2.0)) * (x3 - x2));

        var b = ((y3 - y2) - a * (Math.Pow(x3, 2.0) - Math.Pow(x2, 2.0) )) / (x3 - x2);

        var c = y3 - a * Math.Pow(x3, 2.0) - b * x3;

        var targetSteps = 26501365;

        var possibilities = a * Math.Pow(targetSteps, 2.0) + b * targetSteps + c;

        return (long)possibilities;
    }



    private long GetPlotCountForSteps(int numSteps)
    {
        var start = _grid.First(p => p.Value == 'S').Key;

        var positions = new HashSet<Vector2>();
        positions.Add(start);

        for (int i = 0; i < numSteps; i++)
        {
            var nextPositions = WalkToNewPositions(positions);
            positions = nextPositions;
        }
        return positions.Count;
    }





    private HashSet<Vector2> WalkToNewPositions(HashSet<Vector2> positions)
    {
        var newPositions = new HashSet<Vector2>();

        foreach (var pos in positions)
        {
            foreach (var dir in MathUtils.OrthogonalDirections)
            {
                var newPos = pos + dir;
                if (GetGround(newPos) != '#')
                {
                    // no stone, can walk here
                    newPositions.Add(newPos);
                }
            }
        }
        return newPositions;
    }





    private char GetGround(Vector2 pos)
    {
        var testPos = new Vector2(mod((int)pos.X, _countX), mod((int)pos.Y, _countY));
        return _grid[testPos];
    }

    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }

}
