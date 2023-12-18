using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day17Lib;

public class HeatLossControl
{
    private Dictionary<Vector2, int> _grid = new();

    private Dictionary<Vector2, PositionData> _gridData = new();

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
                _grid.Add(pos, int.Parse(row[x].ToString()));
            }
        }
        _maxX = (int)_grid.Max(p => p.Key.X);
        _maxY = (int)_grid.Max(p => p.Key.Y);
    }

    public int GetLowestHeatLoss()
    {
        var bestMove = GetLowestHeatLossMove();
        return bestMove.Loss;
    }

    public Move GetLowestHeatLossMove()
    {
        var availableMoves = new List<Move>();

        // starting moves
        var startPos = new Vector2(0, 0);
        var endPos = new Vector2(_maxX, _maxY);

        var possibleDirections = new List<Vector2>()
        {
            new Vector2(1, 0),
            new Vector2(0, 1)
        };

        foreach (var dir in possibleDirections)
        {
            var possibleMove = new Move()
            {
                StartPosition = startPos,
                EndPosition = startPos + dir,
                Direction = dir,
                StraightCount = 1,
                Loss = _grid[startPos + dir]
            };
            availableMoves.Add(possibleMove);
        }

        // go through available moves
        while (availableMoves.Count > 0)
        {
            // move with lowest loss
            var currentMove = availableMoves.OrderBy(m => m.Loss).FirstOrDefault();
            availableMoves.Remove(currentMove);

            var curentPosition = currentMove.EndPosition;
            if (!_gridData.TryGetValue(curentPosition, out var currentTileData))
            {
                currentTileData = new PositionData();
                _gridData[curentPosition] = currentTileData;
            }

            var movesInDir = currentTileData.Moves.Where(m => m.Direction == currentMove.Direction);

            // we've reached this before from this direction with lower loss and lower or equal remaining stright steps
            if (movesInDir.Any(m => m.Loss <= currentMove.Loss && m.StraightCount <= currentMove.StraightCount))
            {
                // disregard this move
                continue;
            }

            // otherwise add to moves on this tile data
            currentTileData.Moves.Add(currentMove);

            foreach (var moveDirection in GetMoveDirections(currentMove))
            {
                var newPos = curentPosition + moveDirection.Direction;

                if (_grid.TryGetValue(newPos, out var newLoss))
                {
                    var possibleMove = new Move()
                    {
                        StartPosition = curentPosition,
                        EndPosition = newPos,
                        Direction = moveDirection.Direction,
                        StraightCount = moveDirection.StraightCount,
                        Loss = currentMove.Loss + newLoss
                    };
                    availableMoves.Add(possibleMove);

                    if (newPos == endPos)
                    {
                        // we have found the destination!
                        return possibleMove;
                    }

                }
            }

        }

        return null;

    }

    private List<MoveDirection> GetMoveDirections(Move currentMove)
    {
        var directions = new List<MoveDirection>()
        {
            new MoveDirection()
            {
                Direction = TurnRight(currentMove.Direction),
                StraightCount = 1
            },
            new MoveDirection()
            {
                Direction = TurnLeft(currentMove.Direction),
                StraightCount = 1
            }
        };
        if (currentMove.StraightCount < 3)
        {
            directions.Add(new MoveDirection()
            {
                Direction = currentMove.Direction,
                StraightCount = currentMove.StraightCount + 1
            });
        }
        return directions;
    }

    private Vector2 TurnRight(Vector2 input)
    {
        if (input == new Vector2(0, -1))
        {
            return new Vector2(1, 0);
        }

        if (input == new Vector2(1, 0))
        {
            return new Vector2(0, 1);
        }

        if (input == new Vector2(0, 1))
        {
            return new Vector2(-1, 0);
        }

        if (input == new Vector2(-1, 0))
        {
            return new Vector2(0, -1);
        }

        return new Vector2(0, 0);
    }


    private Vector2 TurnLeft(Vector2 input)
    {
        if (input == new Vector2(0, -1))
        {
            return new Vector2(-1, 0);
        }

        if (input == new Vector2(-1, 0))
        {
            return new Vector2(0, 1);
        }

        if (input == new Vector2(0, 1))
        {
            return new Vector2(1, 0);
        }

        if (input == new Vector2(1, 0))
        {
            return new Vector2(0, -1);
        }

        return new Vector2(0, 0);
    }

}
