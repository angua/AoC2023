using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day11Lib;

public class Astronomy
{
    private Dictionary<Vector2, SpacePosition> _originalSpace = new();
    private List<Galaxy> _originalGalaxies = new();

    private long _expansion = 1000000;

    // empty rows and columns expanded once
    private Dictionary<Vector2, SpacePosition> _yExpandedUniverse = new();
    private Dictionary<Vector2, SpacePosition> _expandedUniverse = new();
    private List<Galaxy> _galaxies = new();

    // distances between galaxies, empty rows and columns expanded once
    // <(first id, second id), distance>
    private Dictionary<(int, int), int> _distances = new();

    // distances between galaxies, empty rows and columns expanded 1000000 times
    private Dictionary<(int, int), long> _wideDistances = new();

   
    private List<int> _emptyRows = new();
    private List<int> _emptyColumns = new();


    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var currentLine = fileData.Lines[y];
            for (int x = 0; x < currentLine.Length; x++)
            {
                var pos = new Vector2(x, y);
                var spacePos = new SpacePosition(currentLine[x]);
                _originalSpace.Add(pos, spacePos);
            }
        }

        // part 1
        ExpandUniverse();
        FindGalaxies();

        FindOriginalGalaxies();
    }

    private void FindOriginalGalaxies()
    {
        var galaxies = _originalSpace.Where(s => s.Value.Type == SpaceType.Galaxy).ToList();

        for (int i = 0; i < galaxies.Count; i++)
        {
            var galaxy = galaxies[i];
            _originalGalaxies.Add(new Galaxy()
            {
                Position = galaxy.Key,
                Id = i,
            });
        }
    }

    private void FindGalaxies()
    {
        var galaxies = _expandedUniverse.Where(s => s.Value.Type == SpaceType.Galaxy).ToList();

        for (int i = 0; i < galaxies.Count; i++)
        {
            var galaxy = galaxies[i];
            _galaxies.Add(new Galaxy()
            {
                Position = galaxy.Key,
                Id = i,
            });
        }
    }

    private void ExpandUniverse()
    {
        // expand in y direction
        var currentY = 0;
        for (int y = 0; y <= _originalSpace.Max(v => v.Key.Y); y++)
        {
            var row = _originalSpace.Where(s => s.Key.Y == y);

            // row gets added once
            foreach (var element in row)
            {
                var newPos = new Vector2(element.Key.X, currentY);
                _yExpandedUniverse.Add(newPos, element.Value);
            }
            currentY++;

            if (!row.Any(s => s.Value.Type == SpaceType.Galaxy))
            {
                _emptyRows.Add(y);
                // empty row gets added twice
                foreach (var element in row)
                {
                    var newPos = new Vector2(element.Key.X, currentY);
                    _yExpandedUniverse.Add(newPos, element.Value);
                }
                currentY++;
            }
        }

        // expand in x direction

        var currentX = 0;
        for (int x = 0; x <= _yExpandedUniverse.Max(v => v.Key.X); x++)
        {
            var column = _yExpandedUniverse.Where(s => s.Key.X == x);

            // column gets added once
            foreach (var element in column)
            {
                var newPos = new Vector2(currentX, element.Key.Y);
                element.Value.Position = newPos;
                _expandedUniverse.Add(newPos, element.Value);
            }
            currentX++;

            if (!column.Any(s => s.Value.Type == SpaceType.Galaxy))
            {
                _emptyColumns.Add(x);
                // empty column gets added twice
                foreach (var element in column)
                {
                    var newPos = new Vector2(currentX, element.Key.Y);
                    element.Value.Position = newPos;
                    _expandedUniverse.Add(newPos, element.Value);
                }
                currentX++;
            }
        }


    }

    public int GetDistanceSum()
    {
        for (int i = 0; i < _galaxies.Count - 1; i++)
        {
            var first = _galaxies[i];

            for (int j = i + 1; j < _galaxies.Count; j++)
            {
                var second = _galaxies[j];

                var dist = GetDistance(first, second);
                _distances.Add((first.Id, second.Id), dist);
            }
        }
        return _distances.Sum(d => d.Value);
    }

    private int GetDistance(Galaxy first, Galaxy second)
    {
        return (int)(Math.Abs(first.Position.X - second.Position.X) + Math.Abs(first.Position.Y - second.Position.Y));
    }

    public long GetWideExpandedDistanceSum()
    {
        for (int i = 0; i < _originalGalaxies.Count - 1; i++)
        {
            var first = _originalGalaxies[i];

            for (int j = i + 1; j < _originalGalaxies.Count; j++)
            {
                var second = _originalGalaxies[j];

                var dist = GetExpandedDistance(first, second);
                _wideDistances.Add((first.Id, second.Id), dist);
            }
        }
        return _wideDistances.Sum(d => d.Value);
    }

    private long GetExpandedDistance(Galaxy first, Galaxy second)
    {
        var minX = (int)(Math.Min(first.Position.X, second.Position.X));
        var maxX = (int)(Math.Max(first.Position.X, second.Position.X));


        long sum = 0;

        for (int x = minX; x < maxX; x++)
        {
            if (_emptyColumns.Contains(x))
            {
                sum += _expansion;
            }
            else
            {
                sum += 1;
            }
        }

        var minY = (int)Math.Min(first.Position.Y, second.Position.Y);
        var maxY = (int)Math.Max(first.Position.Y, second.Position.Y);

        for (int y = minY; y < maxY; y++)
        {
            if (_emptyRows.Contains(y))
            {
                sum += 1000000;
            }
            else
            {
                sum += 1;
            }
        }
        return sum;
    }
}
