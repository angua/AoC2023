using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day03Lib
{
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

        public List<int> FindPartNumbers()
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
            var enginePartNumbers = new List<int>();
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
                    var numStr = string.Join("", num.Positions.Values);
                    enginePartNumbers.Add(int.Parse(numStr));
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

        public int GetEnginePartSum(List<int>partNumbers)
        {
            return partNumbers.Sum();
        }
    }
}
