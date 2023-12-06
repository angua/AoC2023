using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day03Lib;

public class Engineering
{
    public Dictionary<Vector2, char> Schematic { get; set; } = new();

    private int _maxX;
    private int _maxY;

    private List<Vector2> _directions = new List<Vector2>()
    {
        new Vector2(-1,-1),
        new Vector2(-1,0),
        new Vector2(-1,1),
        new Vector2(0,-1),
        new Vector2(0,1),
        new Vector2(1,-1),
        new Vector2(1,0),
        new Vector2(1,1),
    };

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var line = fileData.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                Schematic.Add(new Vector2(x, y), line[x]);
            }
        }
        _maxX = (int)Schematic.Max(p => p.Key.X);
        _maxY = (int)Schematic.Max(p => p.Key.Y);
    }

    public List<SchematicNumber> FindPartNumbers()
    {
        // find all numbers
        var numbers = new List<SchematicNumber>();

        for (int y = 0; y <= _maxY; y++)
        {
            for (int x = 0; x <= _maxX; x++)
            {
                var pos = new Vector2(x, y);

                if (Char.IsDigit(Schematic[pos]))
                {
                    // number 
                    var num = new SchematicNumber();
                    while (Char.IsDigit(Schematic[pos]))
                    {
                        num.Positions.Add(pos, Schematic[pos]);
                        x++;
                        pos = new Vector2(x, y);
                        if (x > _maxX)
                        {
                            break;
                        }
                    }
                    numbers.Add(num);
                }
            }
        }

        // go through numbers and see if their neighbor is an engine part
        var enginePartNumbers = new List<SchematicNumber>();
        foreach (var num in numbers)
        {
            var neighbors = FindNeighbors(num);

            var include = false;
            foreach (var neighbor in neighbors)
            {
                if (neighbor.Value != '.' && !Char.IsDigit(neighbor.Value))
                {
                    // engine part
                    include = true;
                    break;
                }
            }

            if (include)
            {
                enginePartNumbers.Add(num);
            }
        }

        return enginePartNumbers;
    }


    private Dictionary<Vector2, char> FindNeighbors(SchematicNumber num)
    {
        var neighborPositions = new HashSet<Vector2>();

        foreach (var pos in num.Positions.Keys)
        {
            foreach (var dir in _directions)
            {
                // positions around the current character
                var newPos = pos + dir;
                // don't add when it is part of the number or outside schematic
                if (!num.Positions.Keys.Contains(newPos) && Schematic.ContainsKey(newPos))
                {
                    neighborPositions.Add(newPos);
                }
            }
        }

        var result = new Dictionary<Vector2, char>();

        foreach (var pos in neighborPositions)
        {
            result.Add(pos, Schematic[pos]);
        }
        return result;
    }

    public int GetEnginePartSum(List<SchematicNumber> partNumbers)
    {
        return partNumbers.Select(n => n.Number).Sum();
    }


    /// <summary>
    /// Gears are "*" characters adjacent to exactly 2 numbers
    /// </summary>
    /// <returns>Position and gear ratio (adjacent schematic numbers multiplied)</returns>
    public Dictionary<Vector2, int> FindGearPositions()
    {
        var starPositions = GetStarPosítions();
        var result = new Dictionary<Vector2, int>();

        foreach (var pos in starPositions)
        {
            var neighborPositions = _directions.Select(d => pos + d).ToList();
            var neighborSchematicNums = new List<SchematicNumber>();

            // find adjacent schematic numbers
            foreach (var neighborPos in neighborPositions)
            {
                if (Char.IsDigit(Schematic[neighborPos]))
                {
                    var num = GetSchematicNumber(neighborPos);

                    // don't add the same number twice
                    if (!neighborSchematicNums.Any(n => n.StartPosition == num.StartPosition))
                    {
                        neighborSchematicNums.Add(num);
                    }
                }
            }

            if (neighborSchematicNums.Count == 2)
            {
                // gear ratio = product of the 2 neighbor numbers
                var gearRatio = neighborSchematicNums[0].Number * neighborSchematicNums[1].Number;
                result.Add(pos, gearRatio);
            }

        }

        return result;

    }

    private SchematicNumber GetSchematicNumber(Vector2 neighborPos)
    {
        var x = neighborPos.X;
        var y = neighborPos.Y;

        var pos = neighborPos;

        // move to the left to find the first position that is not part of the number
        while (Char.IsDigit(Schematic[pos]))
        {
            x--;
            pos = new Vector2(x, y);
            if (x < 0)
            {
                break;
            }
        }

        // startposition
        x++;
        pos = new Vector2(x, y);

        // move to the right creating the SchematicNumber until we find the first position that is not part of the number
        var num = new SchematicNumber();
        while (Char.IsDigit(Schematic[pos]))
        {
            num.Positions.Add(pos, Schematic[pos]);

            x++;
            pos = new Vector2(x, y);
            if (x > _maxX)
            {
                break;
            }
        }

        return num;
    }

    // Find all positions of * characters in the schematic
    public List<Vector2> GetStarPosítions()
    {
        return Schematic.Where(p => p.Value == '*').Select(p => p.Key).ToList();
    }

    public int GetGearRatioSum(Dictionary<Vector2, int> gearPositions)
    {
        return gearPositions.Sum(p => p.Value);
    }
}
