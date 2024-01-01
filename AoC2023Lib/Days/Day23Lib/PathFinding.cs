﻿using System.Numerics;
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
        FindNeighborsWithoutSlopes();
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

    private void FindNeighborsWithoutSlopes()
    {
        foreach (var tile in _grid)
        {
            var validDirections = new List<Vector2>();
            if (tile.Value.Ground == '#')
            {
                // ignore forest
                continue;
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
                    else
                    {
                        // normal ground "."
                        tile.Value.NeighborsWithoutSlopes.Add(newTile);
                    }
                }
            }
        }
    }




    public long FindLongestPath(bool useSlopes = true)
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

            var neighbors = useSlopes ? currentTile.Neighbors : currentTile.NeighborsWithoutSlopes;

            foreach (var neighbor in neighbors)
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


    public long FindLongestPathWithoutSlopes()
    {
        // get connections between crossroads
        FindConnections();

        var crossroads = _grid.Where(t => t.Value.Connections.Count > 0);



        // move from one connection to the left trying all possibilities
        var start = _grid.Where(p => p.Key.Y == 0 && p.Value.Ground == '.').First();
        var end = _grid.Where(p => p.Key.Y == _maxY && p.Value.Ground == '.').First();

        // all connections from start
        var paths = new Queue<List<ConnectionPath>>();
        var pathsToEnd = new List<List<ConnectionPath>>();

        foreach (var connection in start.Value.Connections)
        {
            var connectionPath = new ConnectionPath
            {
                Start = start.Value,
                End = connection.Crossroads.Where(c => c != start.Value).First(),
                Connection = connection
            };


            paths.Enqueue(new List<ConnectionPath> { connectionPath });
        }

        while (paths.Count > 0)
        {
            var currentPath = paths.Dequeue();

            var currentCrossroad = currentPath.Last().End;

            foreach (var connection in currentCrossroad.Connections)
            {
                var otherEnd = connection.Crossroads.Where(c => c != currentCrossroad).First();

                if (currentPath.Any(p => p.Start == otherEnd || p.End == otherEnd))
                {
                    // don't walk back
                }
                else
                {
                    var newPath = new List<ConnectionPath>(currentPath);
                    var connectionPath = new ConnectionPath
                    {
                        Start = currentCrossroad,
                        End = otherEnd,
                        Connection = connection
                    };
                    newPath.Add(connectionPath);

                    if (otherEnd == end.Value)
                    {
                        // reached destination
                        pathsToEnd.Add(newPath);
                    }
                    else
                    {
                        paths.Enqueue(newPath);
                    }
                }
            }

        }

        var longestPath = pathsToEnd.Max(p => p.Sum(c => c.Connection.StepCount));

        return longestPath;

    }

    private void FindConnections()
    {
        var start = _grid.Where(p => p.Key.Y == 0 && p.Value.Ground == '.').First();
        var end = _grid.Where(p => p.Key.Y == _maxY && p.Value.Ground == '.').First();

        var crossroads = new Queue<Tile>();

        crossroads.Enqueue(start.Value);

        while (crossroads.Count > 0)
        {
            var currentCrossroad = crossroads.Dequeue();

            var routes = new Stack<Route>();

            var startRoute = new Route();
            startRoute.Steps.Add(currentCrossroad);
            routes.Push(startRoute);

            while (routes.Count > 0)
            {
                var currentRoute = routes.Pop();
                var currentTile = currentRoute.Steps.Last();

                foreach (var neighbor in currentTile.NeighborsWithoutSlopes)
                {
                    if (currentRoute.Steps.Contains(neighbor))
                    {
                        // don't step back
                        continue;
                    }
                    if (currentCrossroad.Connections.Any(c => c.Path.Steps.Any(s => s == neighbor)))
                    {
                        // already found this
                        continue;
                    }

                    // not in path already, create new route
                    var newRoute = new Route();
                    newRoute.Steps = new List<Tile>(currentRoute.Steps);
                    newRoute.Steps.Add(neighbor);

                    if (neighbor.NeighborsWithoutSlopes.Count > 2 || neighbor == end.Value || neighbor == start.Value)
                    {
                        // reached crossroad
                        crossroads.Enqueue(neighbor);
                        var connection = new Connection()
                        {
                            Path = newRoute,
                            StepCount = newRoute.Steps.Count - 1,
                            Crossroads = new List<Tile> { currentCrossroad, neighbor }
                        };
                        currentCrossroad.Connections.Add(connection);
                        neighbor.Connections.Add(connection);
                    }
                    // check if we have found this connection already
                    else
                    {
                        // move on 
                        routes.Push(newRoute);
                    }


                }
            }
        }
    }
}

