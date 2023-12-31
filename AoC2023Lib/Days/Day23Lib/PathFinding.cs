using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day23Lib;

public class PathFinding
{
    private Dictionary<Vector2, Tile> _grid = new();

    private int _maxX;
    private int _maxY;

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var line = fileData.Lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                var pos = new Vector2(x, y);
                var tile = new Tile(line[x]);
                tile.Position = pos;
                _grid[pos] = tile;
            }
        }

        _maxX = (int)_grid.Max(p => p.Key.X);
        _maxY = (int)_grid.Max(p => p.Key.Y);

        FindNeighbors();
    }

    private void FindNeighbors()
    {
        foreach (var tile in _grid)
        {
            var validDirections = new List<Vector2>();
            if (tile.Value.Ground == '#')
            {
                // ignore forest
                continue;
            }

            else if (tile.Value.Ground == '<')
            {
                // can only go left from here
                validDirections.Add(new Vector2(-1, 0));
            }

            else if (tile.Value.Ground == '>')
            {
                // can only go right from here
                validDirections.Add(new Vector2(1, 0));
            }

            else if (tile.Value.Ground == '^')
            {
                // can only go up from here
                validDirections.Add(new Vector2(0, -1));
            }

            else if (tile.Value.Ground == 'v')
            {
                // can only go down from here
                validDirections.Add(new Vector2(0, 1));
            }

            else
            {
                // can go in all 4 directions
                validDirections = MathUtils.OrthogonalDirections;
            }

            // check destinations
            foreach (var direction in validDirections)
            {
                var newPos = tile.Key + direction;

                if (_grid.TryGetValue(newPos, out var newTile))
                {
                    if (newTile.Ground == '#')
                    {
                        // ignore forest
                    }
                    else if (newTile.Ground == '<')
                    {
                        if (direction == new Vector2(-1, 0))
                        {
                            tile.Value.Neighbors.Add(newTile);
                        }
                    }
                    else if (newTile.Ground == '>')
                    {
                        if (direction == new Vector2(1, 0))
                        {
                            tile.Value.Neighbors.Add(newTile);
                        }
                    }
                    else if (newTile.Ground == '^')
                    {
                        if (direction == new Vector2(0, -1))
                        {
                            tile.Value.Neighbors.Add(newTile);
                        }
                    }
                    else if (newTile.Ground == 'v')
                    {
                        if (direction == new Vector2(0, 1))
                        {
                            tile.Value.Neighbors.Add(newTile);
                        }
                    }
                    else
                    {
                        // normal ground "."
                        tile.Value.Neighbors.Add(newTile);
                    }
                }
            }
        }
    }


    public long FindLongestPath()
    {
        var start = _grid.Where(p => p.Key.Y == 0 && p.Value.Ground == '.').First();
        var end = _grid.Where(p => p.Key.Y == _maxY && p.Value.Ground == '.').First();

        var routes = new Stack<Route>();

        var finalRoutes = new List<Route>();

        var startRoute = new Route();
        startRoute.Steps.Add(start.Value);
        routes.Push(startRoute);

        while (routes.Count > 0) 
        {
            var currentRoute = routes.Pop();
            var currentTile = currentRoute.Steps.Last();

            foreach (var neighbor in currentTile.Neighbors)
            {
                if (!currentRoute.Steps.Contains(neighbor))
                {
                    // not in path already, create new route
                    var newRoute = new Route();
                    newRoute.Steps = new List<Tile>(currentRoute.Steps);
                    newRoute.Steps.Add(neighbor);

                    if (neighbor == end.Value)
                    {
                        // reached end
                        finalRoutes.Add(newRoute);
                    }
                    else
                    {
                        // move on 
                        routes.Push(newRoute);
                    }
                }
            }

        }

        return finalRoutes.Max(r => r.Steps.Count) - 1;
    }

}
