using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day13Lib;

public class Pattern
{
    public int Id { get; set; }

    private Dictionary<int, string> _rows = new();
    private Dictionary<int, string> _columns = new();

    private Dictionary<int, byte[]> _byteRows = new();
    private Dictionary<int, byte[]> _byteColumns = new();

    private double? _rowMirror = null;
    private double? _columnMirror = null;

    private double? _rowSmudgedMirror = null;
    private double? _columnSmudgedMirror = null;

    private int _columnLeftOfMirror;
    private int _rowsAboveMirror;

    private int _columnLeftOfSmudgedMirror;
    private int _rowsAboveSmudgedMirror;

    public Dictionary<Vector2, byte> Grid { get; private set; } = new();

    private int _maxX;
    private int _maxY;

    private Vector2 _flipPosition;

    internal void AddLine(string line)
    {
        _rows.Add(_rows.Count, line);
    }

    internal void CreateRowsAndColumns()
    {
        // create string columns
        for (int i = 0; i < _rows[0].Length; i++)
        {
            var columnstring = _rows.Select(r => r.Value[i]);
            _columns[i] = string.Join("", columnstring);
        }

        // create byte arrays
        _byteRows = CreateArrays(_rows);
        _byteColumns = CreateArrays(_columns);

        for (int y = 0; y < _byteRows.Count; y++)
        {
            var row = _byteRows[y];
            for (int x = 0; x < row.Length; x++)
            {
                var pos = new Vector2(x, y);
                Grid[pos] = row[x];
            }
        }
        _maxX = (int)Grid.Max(p => p.Key.X);
        _maxY = _byteRows.Count - 1;
    }

    private Dictionary<int, byte[]> CreateArrays(Dictionary<int, string> line)
    {
        var arrays = new Dictionary<int, byte[]>();

        for (int i = 0; i < line.Count; i++)
        {
            var currentLine = line[i];
            var array = new byte[currentLine.Length];

            for (int c = 0; c < currentLine.Length; c++)
            {
                var currentChar = currentLine[c];
                array[c] = GetByte(currentChar);
            }
            arrays.Add(i, array);
        }
        return arrays;
    }


    public int GetMirrorSum()
    {
        FindMirror();
        return _columnLeftOfMirror + 100 * _rowsAboveMirror;
    }

    private void FindMirror()
    {
        // try finding mirror in rows and columns
        var rowMirrors = TryFindMirrors(_byteRows);

        if (rowMirrors.Count > 0)
        {
            _rowMirror = rowMirrors.First();
        }

        var stringrowMirror = TryFindMirror(_rows);


        var columnMirrors = TryFindMirrors(_byteColumns);
        if (columnMirrors.Count > 0)
        {
            _columnMirror = columnMirrors.First();
        }
        var stringColumnMirror = TryFindMirror(_columns);


        // zero based counting of rows
        if (_rowMirror != null)
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



    private byte GetByte(char currentChar)
    {
        return currentChar switch
        {
            '.' => 0,
            '#' => 1,
            _ => throw new InvalidOperationException()
        };
    }

    internal int GetSmudgedMirrorSum()
    {
        FindSmudgedMirror();
        return _columnLeftOfSmudgedMirror + 100 * _rowsAboveSmudgedMirror;
    }

    private void FindSmudgedMirror()
    {
        // go through grid and change each position
        for (int y = 0; y <= _maxY; y++)
        {
            if (_columnSmudgedMirror != null || _rowSmudgedMirror != null)
            {
                break;
            }

            for (int x = 0; x <= _maxX; x++)
            {
                // copy grid
                var pos = new Vector2(x, y);
                var testGrid = new Dictionary<Vector2, byte>(Grid);

                // change position
                var groundAtPos = testGrid[pos];
                testGrid[pos] = Flip(groundAtPos);

                // test for mirror in rows
                var rows = new Dictionary<int, byte[]>();
                for (int currentY = 0; currentY <= _maxY; currentY++)
                {
                    var rowElements = testGrid.Where(e => e.Key.Y == currentY).OrderBy(e => e.Key.X).Select(e => e.Value);
                    rows.Add(currentY, rowElements.ToArray());
                }
                var rowSmudgedMirrors = TryFindMirrors(rows);

                if (rowSmudgedMirrors.Count > 0)
                {
                    foreach (var mirror in rowSmudgedMirrors)
                    {
                        if (mirror != _rowMirror)
                        {
                            // found
                            _rowSmudgedMirror = mirror;
                            _flipPosition = pos;
                            break;
                        }
                    }
                }


                // test for mirror in columns
                var columns = new Dictionary<int, byte[]>();
                for (int currentX = 0; currentX <= _maxX; currentX++)
                {
                    var columnElements = testGrid.Where(e => e.Key.X == currentX).OrderBy(e => e.Key.Y).Select(e => e.Value);
                    columns.Add(currentX, columnElements.ToArray());
                }

                var columnSmudgedMirrors = TryFindMirrors(columns);

                if (columnSmudgedMirrors.Count > 0)
                {
                    foreach (var mirror in columnSmudgedMirrors)
                    {
                        if (mirror != _columnMirror)
                        {
                            // found
                            _columnSmudgedMirror = mirror;
                            _flipPosition = pos;
                            break;
                        }
                    }
                }
            }
        }


        // zero based counting of rows
        if (_rowSmudgedMirror != null)
        {
            _rowsAboveSmudgedMirror = (int)Math.Floor((double)_rowSmudgedMirror) + 1;
        }

        if (_columnSmudgedMirror != null)
        {
            _columnLeftOfSmudgedMirror = (int)Math.Floor((double)_columnSmudgedMirror) + 1;
        }
    }

    private byte Flip(byte input)
    {
        return input switch
        {
            0 => 1,
            1 => 0
        };
    }

    private HashSet<double> TryFindMirrors(Dictionary<int, byte[]> lines)
    {
        // go through all line combinations
        var combinations = MathUtils.GetAllCombinations(lines.Count, 2);

        var result = new HashSet<double>();

        var diffs = new Dictionary<(int, int), byte[]>();
        var diffSums = new Dictionary<(int, int), int>();

        foreach (var combination in combinations)
        {
            var firstId = combination[0];
            var secondId = combination[1];
            var firstLine = lines[firstId];
            var secondLine = lines[secondId];

            // difference between lines
            var diff = GetDiff(firstLine, secondLine);

            // sum of different positions
            // 0 when lines are equal
            var diffSum = GetDiffSum(diff);

            diffs[(firstId, secondId)] = diff;
            diffSums[(firstId, secondId)] = diffSum;
        }

        var equalPairs = diffSums.Where(s => s.Value == 0).Select(p => p.Key).OrderBy(p => p.Item1);

        foreach (var pair in equalPairs)
        {
            // check if mirror works for this
            var center = (double)(pair.Item2 + pair.Item1) / 2;

            // disregard if not between lines
            if (center - Math.Floor(center) == 0.5)
            {
                var isMirror = true;

                var isInLines = true;
                var testOffset = 0;

                while (isInLines)
                {
                    // lines on both sides of the mirror must be equal
                    var testPos1 = (int)(center - 0.5 - testOffset);
                    var testPos2 = (int)(center + 0.5 + testOffset);

                    // outside grid
                    if (!lines.ContainsKey(testPos1) || !lines.ContainsKey(testPos2))
                    {
                        isInLines = false;
                        break;
                    }

                    var testPair = (testPos1, testPos2);

                    if (!equalPairs.Contains(testPair))
                    {
                        isMirror = false;
                        break;
                    }

                    testOffset++;
                }

                if (isMirror)
                {
                    result.Add(center);
                }
            }
        }

        return result;
    }

    private int GetDiffSum(byte[] diff)
    {
        var sum = 0;

        foreach (var pos in diff)
        {
            sum += pos;
        }
        return sum;
    }

    private byte[] GetDiff(byte[] firstLine, byte[] secondLine)
    {
        var diff = new byte[firstLine.Length];
        for (int i = 0; i < firstLine.Length; i++)
        {
            diff[i] = (byte)(firstLine[i] - secondLine[i]);
        }
        return diff;
    }
}
