using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day13Lib;

public class Pattern
{
    private Dictionary<int, string> _rows = new();
    private Dictionary<int, string> _columns = new();

    // reverse lookup <ground string, row ids>
    private Dictionary<string, List<int>> _equalRows = new();
    private Dictionary<string, List<int>> _equalColumns = new();

    private double? _rowMirror = null;
    private double? _columnMirror = null;

    private int _columnLeftOfMirror;
    private int _rowsAboveMirror;


    internal void AddLine(string line)
    {
        _rows.Add(_rows.Count, line);
    }

    internal void CreateColumns()
    {
        for (int i = 0; i < _rows[0].Length; i++)
        {
            var columnstring = _rows.Select(r => r.Value[i]);
            _columns[i] = string.Join("", columnstring);
        }
    }

    public int GetMirrorSum()
    {
        FindMirror();

        return _columnLeftOfMirror + 100 * _rowsAboveMirror;

    }

    private void FindMirror()
    {
        // try finding mirror in rows and columns
        _rowMirror = TryFindMirror(_rows);
        _columnMirror = TryFindMirror(_columns);

        // zero based counting of rows
        if (_rowMirror != null ) 
        {
            _rowsAboveMirror = (int)Math.Floor((double)_rowMirror) + 1;
        }

        if (_columnMirror != null)
        {
            _columnLeftOfMirror = (int)Math.Floor((double)_columnMirror) + 1;
        }

    }

    private double? TryFindMirror(Dictionary<int, string> lines)
    {
        var equalLines = new Dictionary<string, List<int>>();
        foreach (var line in lines)
        {
            if (!equalLines.TryGetValue(line.Value, out var lineIds))
            {
                lineIds = new List<int>();
                equalLines.Add(line.Value, lineIds);
            }
            lineIds.Add(line.Key);
        }

        var duplicatedLines = equalLines.Where(r => r.Value.Count > 1);
        var linePairs = new List<(int, int)>();
        foreach (var mirrored in duplicatedLines)
        {
            if (mirrored.Value.Count > 2)
            {
                var combinations = MathUtils.GetAllCombinations(mirrored.Value.Count, 2);
                foreach (var combination in combinations)
                {
                    linePairs.Add((mirrored.Value[combination[0]], mirrored.Value[combination[1]]));
                }
            }
            else
            {
                linePairs.Add((mirrored.Value[0], mirrored.Value[1]));
            }
        }

        // < line id pair, center>
        var centers = new Dictionary<(int, int), double>();
        foreach (var linePair in linePairs)
        {
            centers.Add(linePair, (double)(linePair.Item1 + linePair.Item2) / 2);
        }

        // inverse: <center, line id pairs>
        var mirroredLines = new Dictionary<double, List<(int, int)>>();
        foreach (var centerData in centers)
        {
            if (!mirroredLines.TryGetValue(centerData.Value, out var pairs))
            {
                pairs = new List<(int, int)>();
                mirroredLines[centerData.Value] = pairs;
            }
            pairs.Add(centerData.Key);
        }

        // check if every line is mirrored
        foreach (var center in mirroredLines.Keys)
        {
            // disregard centers that are not between rows
            if (center - Math.Floor(center) == 0.5)
            {
                var currentPairs = mirroredLines[center];

                var isMirror = true;

                var isInLines = true;
                var testOffset = 0;

                while (isInLines)
                {
                    var testPos1 = (int)(center + 0.5 + testOffset);
                    var testPos2 = (int)(center - 0.5 - testOffset);

                    if (!lines.ContainsKey(testPos1) || !lines.ContainsKey(testPos2))
                    {
                        isInLines = false;
                        break;
                    }

                    if (!currentPairs.Any(p => p.Item1 == testPos1 || p.Item2 == testPos1))
                    {
                        isMirror = false;
                        break;
                    }
                    if (!currentPairs.Any(p => p.Item1 == testPos2 || p.Item2 == testPos2))
                    {
                        isMirror = false;
                        break;
                    }

                    testOffset++;
                }

                if (isMirror)
                {
                    return center;
                }
            }

        }

        return null;
    }

    private int GetInt(char currentChar)
    {
        return currentChar switch
        {
            '.' => 0,
            '#' => 1,
            _ => throw new InvalidOperationException()
        };
    }
}
