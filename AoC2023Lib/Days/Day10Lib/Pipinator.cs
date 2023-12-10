using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day10Lib;

public class Pipinator
{
    private Dictionary<Vector2, Pipe> _pipes = new();

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var line = fileData.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var pos = new Vector2(x, y);
                var pipe = line[x];

                _pipes.Add(pos, new Pipe()
                {
                    Position = pos,
                    Orientation = GetOrientation(pipe)
                });
            }
        }

        GetNeighbors();
    }

    private void GetNeighbors()
    {
        foreach (var pipe in _pipes)
        {
            pipe.Value.Directions = Pipe.GetNeighborDirections(pipe.Value);
        }

        foreach (var pipe in _pipes)
        {
            if (pipe.Value.Directions != null)
            {
                foreach (var direction in pipe.Value.Directions)
                {
                    var newPos = pipe.Key + direction;

                    if (_pipes.TryGetValue(newPos, out var newPipe))
                    {
                        if (newPipe.Orientation != PipeOrientation.None)
                        {
                            var inverseDirection = -1 * direction;
                            if (newPipe.Orientation == PipeOrientation.Start || newPipe.Directions.Contains(inverseDirection))
                            {
                                // connect
                                if (!pipe.Value.Neighbors.Contains(newPipe))
                                {
                                    pipe.Value.Neighbors.Add(newPipe);
                                }
                                if (!newPipe.Neighbors.Contains(pipe.Value))
                                {
                                    newPipe.Neighbors.Add(pipe.Value);
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    public int GetLongestDistanceFromStart()
    {
        var start = _pipes.First(p => p.Value.Orientation == PipeOrientation.Start).Value;
        var distance = 0;

        start.DistanceFomStart = distance;

        // neighbors from start
        distance++;
        var newPipes = start.Neighbors;
        foreach (var pipe in newPipes)
        {
            pipe.DistanceFomStart = distance;
        }

        while (true)
        {
            // move to next neighbors
            distance++;
            var allNeighbors = new List<Pipe>();
            foreach (var pipe in newPipes)
            {
                var neighbors = pipe.Neighbors.Where(n => n.DistanceFomStart == null);
                allNeighbors.AddRange(neighbors);
            }
            foreach (var neighbor in allNeighbors)
            {
                neighbor.DistanceFomStart = distance;
            }

            if (allNeighbors.Distinct().Count() == 1)
            {
                // same element from both sides
                break;
            }
            newPipes = allNeighbors;

        }

        return (int)_pipes.Max(p => p.Value.DistanceFomStart);


    }

    private PipeOrientation GetOrientation(char pipe)
    {
        return pipe switch
        {
            '.' => PipeOrientation.None,
            '|' => PipeOrientation.NorthSouth,
            '-' => PipeOrientation.WestEast,
            'L' => PipeOrientation.NorthEast,
            'J' => PipeOrientation.NorthWest,
            '7' => PipeOrientation.SouthWest,
            'F' => PipeOrientation.SouthEast,
            'S' => PipeOrientation.Start,
            _ => throw new InvalidOperationException()
        };
    }
}
