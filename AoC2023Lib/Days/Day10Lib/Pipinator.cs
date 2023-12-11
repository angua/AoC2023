using System.Diagnostics;
using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day10Lib;

public class Pipinator
{
    private Dictionary<Vector2, Pipe> _pipes = new();

    private List<Vector2> _directions = new List<Vector2>()
    {
        new Vector2(0, -1),
        new Vector2(1, 0),
        new Vector2(0, 1),
        new Vector2(-1, 0),
    };

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
            foreach (var direction in pipe.Value.Directions)
            {
                var newPos = pipe.Key + direction;

                if (_pipes.TryGetValue(newPos, out var newPipe))
                {
                    var inverseDirection = -1 * direction;
                    if (newPipe.Orientation == PipeOrientation.Start || newPipe.Directions.Contains(inverseDirection))
                    {
                        // connect
                        if (!pipe.Value.Neighbors.Values.Contains(newPipe))
                        {
                            pipe.Value.Neighbors.Add(direction, newPipe);
                        }
                        if (!newPipe.Neighbors.Values.Contains(pipe.Value))
                        {
                            newPipe.Neighbors.Add(inverseDirection, pipe.Value);
                        }

                        if (newPipe.Orientation == PipeOrientation.Start)
                        {
                            newPipe.Directions.Add(inverseDirection);
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
        var newPipes = start.Neighbors.Select(n => n.Value);
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
                var neighbors = pipe.Neighbors.Where(n => n.Value.DistanceFomStart == null).Select(n => n.Value);
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

    public HashSet<Pipe> GetEnclosedTiles()
    {
        var outside = Side.Unknown;

        var rightPositions = new HashSet<Vector2>();
        var leftPositions = new HashSet<Vector2>();

        // move to random neighbor from start
        var currentPipe = _pipes.First(p => p.Value.Orientation == PipeOrientation.Start).Value;

        var nextPipePos = currentPipe.Neighbors.First();

        while (true)
        {
            // test directions are orthogonal to move direction
            var rightDir = TurnRight(nextPipePos.Key);
            var leftDir = -1 * rightDir;

            // test positions orthogonal to current tile
            outside = TestOrthogonalPositions(outside, rightPositions, leftPositions, currentPipe, rightDir, leftDir);

            var previousPos = currentPipe.Position;
            currentPipe = nextPipePos.Value;

            // test positions orthogonal to next tile
            outside = TestOrthogonalPositions(outside, rightPositions, leftPositions, currentPipe, rightDir, leftDir);

            // move on to next pipe
            nextPipePos = currentPipe.Neighbors.Where(p => p.Value.Position != previousPos).First();

            if (nextPipePos.Value.Orientation == PipeOrientation.Start)
            {
                // done with loop
                break;
            }
        }

        if (outside != Side.Right) 
        {
            return rightPositions.Select(v => _pipes[v]).ToHashSet();
        }
        else
        {
            return leftPositions.Select(v => _pipes[v]).ToHashSet();
        }
    }

    private Side TestOrthogonalPositions(Side outside, HashSet<Vector2> rightPositions, HashSet<Vector2> leftPositions, Pipe currentPipe, Vector2 rightDir, Vector2 leftDir)
    {
        var rightTestPos = currentPipe.Position + rightDir;
        var leftTestPos = currentPipe.Position + leftDir;

        var newRightPositions = FindPositions(rightTestPos, out var rightIsOutside);
        foreach (var pos in newRightPositions)
        {
            rightPositions.Add(pos);
        }

        if (rightIsOutside)
        {
            outside = Side.Right;
        }

        var newLeftPositions = FindPositions(rightTestPos, out var leftIsOutside);
        foreach (var pos in newLeftPositions)
        {
            leftPositions.Add(pos);
        }

        if (leftIsOutside)
        {
            outside = Side.Left;
        }

        return outside;
    }

    private HashSet<Vector2> FindPositions(Vector2 startPos, out bool isOutside)
    {
        isOutside = false;

        var availablePositions = new HashSet<Vector2>();
        var visitedPositions = new HashSet<Vector2>();

        var foundPositions = new HashSet<Vector2>();

        availablePositions.Add(startPos);

        while (availablePositions.Count > 0)
        {
            var testPos = availablePositions.First();

            if (_pipes.TryGetValue(testPos, out var pipe))
            {
                if (!PartOfLoop(pipe))
                {
                    // this is not part of the loop, add to list and flood fill
                    foundPositions.Add(testPos);

                    foreach (var dir in _directions)
                    {
                        var newPos = testPos + dir;
                        if (!visitedPositions.Contains(newPos))
                        {
                            availablePositions.Add(newPos);
                        }
                    }
                }
            }
            else
            {
                // not in grid, reached outside
                isOutside = true;
            }
            availablePositions.Remove(testPos);
            visitedPositions.Add(testPos);
        }

        return foundPositions;
    }

    private bool PartOfLoop(Pipe pipe)
    {
        return pipe.DistanceFomStart != null;
    }

    private Vector2 TurnRight(Vector2 input)
    {
        if (input == new Vector2(0, -1))
        {
            return new Vector2(1, 0);
        }

        if (input == new Vector2(1, 0))
        {
            return new Vector2(0, 1);
        }

        if (input == new Vector2(0, 1))
        {
            return new Vector2(-1, 0);
        }

        if (input == new Vector2(-1, 0))
        {
            return new Vector2(0, -1);
        }

        return new Vector2(0, 0);
    }


    private Vector2 TurnLeft(Vector2 input)
    {
        if (input == new Vector2(0, -1))
        {
            return new Vector2(-1, 0);
        }

        if (input == new Vector2(-1, 0))
        {
            return new Vector2(0, 1);
        }

        if (input == new Vector2(0, 1))
        {
            return new Vector2(1, 0);
        }

        if (input == new Vector2(1, 0))
        {
            return new Vector2(0, -1);
        }

        return new Vector2(0, 0);
    }



    public int GetEnclosedTilesCount()
    {
        return GetEnclosedTiles().Count();
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
