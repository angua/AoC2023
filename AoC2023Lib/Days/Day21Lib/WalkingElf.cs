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
    }

    public long GetPlots64Steps()
    {
        return GetPlotsForSteps(64);
    }

    private long GetPlotsForSteps(int numSteps)
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
                if (_grid[newPos] != '#')
                {
                    // no stone, can walk here
                    newPositions.Add(newPos);
                }
            }
        }

        return newPositions;
    }
}
