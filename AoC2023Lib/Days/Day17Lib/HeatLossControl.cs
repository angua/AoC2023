﻿using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day17Lib;

public class HeatLossControl
{
    public Dictionary<Vector2, int> Grid { get; private set; } = new();

    private int _maxX;
    private int _maxY;

    public int CountX => _maxX + 1;
    public int CountY => _maxY + 1;

    public Move BestMove { get; private set; }
    public Move BestUltraMove { get; private set; }

    public void Parse(Filedata fileData)
    {
        for (int y = 0; y < fileData.Lines.Count; y++)
        {
            var row = fileData.Lines[y];
            for (int x = 0; x < row.Length; x++)
            {
                var pos = new Vector2(x, y);
                Grid.Add(pos, int.Parse(row[x].ToString()));
            }
        }
        _maxX = (int)Grid.Max(p => p.Key.X);
        _maxY = (int)Grid.Max(p => p.Key.Y);
    }

    public int GetLowestHeatLoss()
    {
        BestMove = GetLowestHeatLossMove();
        return BestMove.Loss;
    }

    public int GetLowestUltraHeatLoss()
    {
        BestUltraMove = GetLowestHeatLossMove(true);
        return BestUltraMove.Loss;
    }

    public Move GetLowestHeatLossMove(bool useUltra = false)
    {
        var availableMoves = new List<Move>();

        var gridData = new Dictionary<Vector2, PositionData>();

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
                EndPosition = startPos + dir,
                Direction = dir,
                StraightCount = 1,
                Loss = Grid[startPos + dir],
                Route = new List<Vector2>
                {
                    startPos,
                }
            };
            availableMoves.Add(possibleMove);
        }

        // go through available moves
        while (availableMoves.Count > 0)
        {
            // move with lowest loss
            var currentMove = availableMoves.OrderBy(m => m.Loss).FirstOrDefault();
            availableMoves.Remove(currentMove);

            var currentPosition = currentMove.EndPosition;
            if (!gridData.TryGetValue(currentPosition, out var currentTileData))
            {
                currentTileData = new PositionData();
                gridData[currentPosition] = currentTileData;
            }

            var movesInDir = currentTileData.Moves.Where(m => m.Direction == currentMove.Direction);
            if (!useUltra)
            {
                // we've reached this before from this direction with lower loss and lower or equal remaining stright steps
                if (movesInDir.Any(m => m.Loss <= currentMove.Loss && m.StraightCount <= currentMove.StraightCount))
                {
                    // disregard this move
                    continue;
                }
            }
            else
            {
                if (movesInDir.Any(m => m.Loss <= currentMove.Loss && m.StraightCount == currentMove.StraightCount))
                {
                    // disregard this move
                    continue;
                }
            }

            // otherwise add to moves on this tile data
            currentTileData.Moves.Add(currentMove);

            foreach (var moveDirection in GetMoveDirections(currentMove, useUltra))
            {
                var newPos = currentPosition + moveDirection.Direction;

                if (Grid.TryGetValue(newPos, out var newLoss))
                {
                    var possibleMove = new Move()
                    {
                        EndPosition = newPos,
                        Direction = moveDirection.Direction,
                        StraightCount = moveDirection.StraightCount,
                        Loss = currentMove.Loss + newLoss
                    };
                    possibleMove.Route = new List<Vector2>(currentMove.Route);
                    possibleMove.Route.Add(currentPosition);

                    availableMoves.Add(possibleMove);

                    if (newPos == endPos)
                    {
                        // we have found the destination!
                        if (!useUltra || useUltra && possibleMove.StraightCount >= 4)
                        {
                            return possibleMove;
                        }
                    }

                }
            }

        }

        return null;
    }


    private List<MoveDirection> GetMoveDirections(Move currentMove, bool useUltra)
    {
        var directions = new List<MoveDirection>();

        if (useUltra)
        {
            if (currentMove.StraightCount < 10)
            {
                directions.Add(new MoveDirection()
                {
                    Direction = currentMove.Direction,
                    StraightCount = currentMove.StraightCount + 1
                });
            }
            if (currentMove.StraightCount >= 4)
            {
                directions.Add(new MoveDirection()
                {
                    Direction = TurnRight(currentMove.Direction),
                    StraightCount = 1
                });
                directions.Add(new MoveDirection()
                {
                    Direction = TurnLeft(currentMove.Direction),
                    StraightCount = 1
                });
            }
        }
        else
        {
            directions.Add(new MoveDirection()
            {
                Direction = TurnRight(currentMove.Direction),
                StraightCount = 1
            });
            directions.Add(new MoveDirection()
            {
                Direction = TurnLeft(currentMove.Direction),
                StraightCount = 1
            });
            if (currentMove.StraightCount < 3)
            {
                directions.Add(new MoveDirection()
                {
                    Direction = currentMove.Direction,
                    StraightCount = currentMove.StraightCount + 1
                });
            }
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