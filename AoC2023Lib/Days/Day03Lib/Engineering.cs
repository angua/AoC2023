using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day03Lib;

public class Engineering
{
    public Dictionary<Vector2, EnginePosition> Schematic { get; set; } = new();

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
                Schematic.Add(new Vector2(x, y), new EnginePosition(line[x]));
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
                var part = Schematic[pos];

                if (Char.IsDigit(part.Symbol))
                {
                    // start of number 
                    var num = new SchematicNumber();

                    // the rest of the number
                    while (Char.IsDigit(Schematic[pos].Symbol))
                    {
                        num.Positions.Add(pos, Schematic[pos].Symbol);
                        
                        // advance to next position
                        x++;
                        pos = new Vector2(x, y);
                        if (x > _maxX)
                        {
                            break;
                        }
                    }

                    numbers.Add(num);
                    foreach (var numPos in num.Positions)
                    {
                        Schematic[numPos.Key].SchematicNumber = num;
                    }
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
                if (neighbor.Value.Symbol != '.' && !Char.IsDigit(neighbor.Value.Symbol))
                {
                    // engine part
                    include = true;
                    break;
                }
            }

            if (include)
            {
                enginePartNumbers.Add(num);

                foreach (var part in num.Positions)
                {
                    Schematic[part.Key].Type = SymbolType.Number;
                }

            }
        }

        return enginePartNumbers;
    }


    private Dictionary<Vector2, EnginePosition> FindNeighbors(SchematicNumber num)
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

        var result = new Dictionary<Vector2, EnginePosition>();

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
                if (Schematic[neighborPos].Type == SymbolType.Number)
                {
                    var num = Schematic[neighborPos].SchematicNumber;

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
                Schematic[pos].Type = SymbolType.Gear;
            }
        }

        return result;
    }

    // Find all positions of * characters in the schematic
    public List<Vector2> GetStarPosítions()
    {
        return Schematic.Where(p => p.Value.Symbol == '*').Select(p => p.Key).ToList();
    }

    public int GetGearRatioSum(Dictionary<Vector2, int> gearPositions)
    {
        return gearPositions.Sum(p => p.Value);
    }
}
