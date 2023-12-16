using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day14Lib;

public class PlatformControl
{
    public Dictionary<Vector2, LocationType> Grid { get; set; } = new();

    private Dictionary<Vector2, LocationType> _startGrid = new();

    private int _maxX;
    private int _maxY;

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var row = fileData.Lines[y];
            for (int x = 0; x < row.Length; x++)
            {
                var pos = new Vector2(x, y);
                var element = row[x];
                _startGrid.Add(pos, GetLocationType(element));
            }
        }
        Grid = new Dictionary<Vector2, LocationType>(_startGrid);
        _maxX = (int)_startGrid.Max(p => p.Key.X);
        _maxY = (int)_startGrid.Max(p => p.Key.Y);
    }

    public int GetLoadAfterNorthTilt()
    {
        // tilt north to let round rocks roll
        Tilt(Direction.North);

        return GetLoad(Direction.North);



    }

    private int GetLoad(Direction direction)
    {
        var load = 0;
        if (direction == Direction.North)
        {
            for (int y = 0; y <= _maxY; y++)
            {
                var row = Grid.Where(p => p.Key.Y == y);
                var roundRocksCount = row.Where(p => p.Value == LocationType.RoundRock).Count();

                var multiplier = _maxY + 1 - y;
                load += roundRocksCount * multiplier;
            }
        }
        return load;
    }

    private void Tilt(Direction direction)
    {
        var moveDir = GetMoveDir(direction);

        if (direction == Direction.North)
        {
            // top row rocks can't move, start with second row
            var startY = 1;

            for (int y = startY; y <= _maxY; y++)
            {
                var row = Grid.Where(p => p.Key.Y == y);
                var RoundRocksInRow = row.Where(p => p.Value == LocationType.RoundRock);

                foreach (var rock in RoundRocksInRow)
                {
                    // move round rock up as far as possible
                    Move(rock, moveDir);
                }
            }

        }
    }

    private void Move(KeyValuePair<Vector2, LocationType> rock, Vector2 moveDir)
    {
        var pos = rock.Key;

        // remove rock from grid
        Grid[pos] = LocationType.None;

        var testPos = pos;

        // move up until we hit other rock or the edge of the grid
        while (Grid.TryGetValue(testPos, out var ground) && ground == LocationType.None)
        {
            pos = testPos;
            testPos = pos + moveDir;
        }

        // place rock at new position
        Grid[pos] = LocationType.RoundRock;
    }

    private Vector2 GetMoveDir(Direction direction)
    {
        return direction switch
        {
            Direction.North => new Vector2(0, -1),
            Direction.South => new Vector2(0, 1),
            Direction.West => new Vector2(-1, 0),
            Direction.East => new Vector2(1, 0)
        };
    }

    private LocationType GetLocationType(char element)
    {
        return element switch
        {
            '.' => LocationType.None,
            '#' => LocationType.SolidRock,
            'O' => LocationType.RoundRock
        };
    }
}
