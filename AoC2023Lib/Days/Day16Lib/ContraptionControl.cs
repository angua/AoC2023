using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day16Lib;

public class ContraptionControl
{
    
    private Dictionary<Vector2, Tile> _startGrid { get; set; } = new();
    public Dictionary<Vector2, Tile> Grid { get; set; } = new();

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
                _startGrid.Add(pos, new Tile()
                {
                    Mirror = GetMirror(row[x])
                });
            }
        }

        _maxX = (int)_startGrid.Max(p => p.Key.X);
        _maxY = (int)_startGrid.Max(p => p.Key.Y);
    }

    private TileMirror GetMirror(char c)
    {
        return c switch
        {
            '.' => TileMirror.None,
            '/' => TileMirror.Slash,
            '\\' => TileMirror.Backslash,
            '|' => TileMirror.VerticalSplitter,
            '-' => TileMirror.HorizontalSplitter
        };
    }

    public int GetBestEnergizedTilesCount()
    {
        var bestEnergizedCount = 0;

        for (int y = 0; y <= _maxY; y++)
        {
            // start left edge
            var energized = GetEnergizedTilesCount(new Vector2(0, y), Direction.Right);
            if (energized > bestEnergizedCount)
            {
                bestEnergizedCount = energized;
            }

            // start right edge
            energized = GetEnergizedTilesCount(new Vector2(_maxX, y), Direction.Left);
            if (energized > bestEnergizedCount)
            {
                bestEnergizedCount = energized;
            }
        }

        for (int x = 0; x <= _maxX; x++)
        {
            // start top edge
            var energized = GetEnergizedTilesCount(new Vector2(x, 0), Direction.Down);
            if (energized > bestEnergizedCount)
            {
                bestEnergizedCount = energized;
            }

            // start right edge
            energized = GetEnergizedTilesCount(new Vector2(x, _maxY), Direction.Up);
            if (energized > bestEnergizedCount)
            {
                bestEnergizedCount = energized;
            }
        }

        return bestEnergizedCount;
    }

    public int GetEnergizedTilesCount()
    {

        var startPos = new Vector2(0, 0);
        var incoming = Direction.Right;

        return GetEnergizedTilesCount(startPos, incoming);
    }


    public int GetEnergizedTilesCount(Vector2 startPos, Direction startIncoming)
    {
        Grid = GenerateGrid();

        var tilesToProcess = new List<(Vector2 position, Direction incoming)>();

        tilesToProcess.Add((startPos, startIncoming));

        while (tilesToProcess.Count > 0)
        {
            var currentTile = tilesToProcess.Last();
            tilesToProcess.Remove(currentTile);

            var outgoing = ProcessTile(currentTile.position, currentTile.incoming);

            foreach (var dir in outgoing)
            {
                var newPos = currentTile.position + GetVector(dir);
                tilesToProcess.Add((newPos, dir));
            }

        }

        return Grid.Count(t => t.Value.IsEnergized == true);
    }

    private Dictionary<Vector2, Tile> GenerateGrid()
    {
        var grid = new Dictionary<Vector2, Tile>(_startGrid);

        foreach (var tile in Grid)
        {
            tile.Value.IsEnergized = false;
            tile.Value.IncomingBeams.Clear();
        }

        return grid;
    }

    private List<Direction> ProcessTile(Vector2 position, Direction incoming)
    {
        var outgoing = new List<Direction>();

        if (Grid.TryGetValue(position, out var tile))
        {
            if (tile.IncomingBeams.Contains(incoming))
            {
                // loop
                return outgoing;
            }

            tile.IsEnergized = true;
            tile.IncomingBeams.Add(incoming);


            switch (tile.Mirror)
            {
                case TileMirror.None:
                    outgoing.Add(incoming);
                    break;

                // "/"
                case TileMirror.Slash:
                    switch (incoming)
                    {
                        case Direction.Right:
                            outgoing.Add(Direction.Up);
                            break;

                        case Direction.Up:
                            outgoing.Add(Direction.Right);
                            break;

                        case Direction.Left:
                            outgoing.Add(Direction.Down);
                            break;

                        case Direction.Down:
                            outgoing.Add(Direction.Left);
                            break;
                    }
                    break;

                // "\"
                case TileMirror.Backslash:
                    switch (incoming)
                    {
                        case Direction.Right:
                            outgoing.Add(Direction.Down);
                            break;

                        case Direction.Up:
                            outgoing.Add(Direction.Left);
                            break;

                        case Direction.Left:
                            outgoing.Add(Direction.Up);
                            break;

                        case Direction.Down:
                            outgoing.Add(Direction.Right);
                            break;
                    }
                    break;

                // "-"
                case TileMirror.HorizontalSplitter:
                    if (incoming == Direction.Left || incoming == Direction.Right)
                    {
                        // pass through
                        outgoing.Add(incoming);
                    }
                    else
                    {
                        // split in 2 parts orthogonal to incoming
                        outgoing.Add(Direction.Left);
                        outgoing.Add(Direction.Right);
                    }
                    break;

                // "|"
                case TileMirror.VerticalSplitter:
                    if (incoming == Direction.Up || incoming == Direction.Down)
                    {
                        // pass through
                        outgoing.Add(incoming);
                    }
                    else
                    {
                        // split in 2 parts orthogonal to incoming
                        outgoing.Add(Direction.Up);
                        outgoing.Add(Direction.Down);
                    }
                    break;
            }
        }
        return outgoing;
    }

    private Vector2 GetVector(Direction dir)
    {
        return dir switch
        {
            Direction.Left => new Vector2(-1, 0),
            Direction.Right => new Vector2(1, 0),
            Direction.Up => new Vector2(0, -1),
            Direction.Down => new Vector2(0, 1)
        };
    }

}
