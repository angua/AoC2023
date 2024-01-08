using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day17Lib;

public class HeatLossMap
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


    public Move GetLowestHeatLossMove(bool useUltra = false)
    {
        // <loss, move>
        var availableMoves = new SortedDictionary<int, List<Move>>();

        var gridData = new Dictionary<Vector2, PositionData>();

        var startPos = new Vector2(0, 0);
        var endPos = new Vector2(_maxX, _maxY);

        // starting moves
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
                    startPos + dir
                }
            };
            AddMove(availableMoves, possibleMove);
        }

        // go through available moves
        while (availableMoves.Any())
        {
            // choose available move with lowest loss
            var minLossMoves = availableMoves.First();
            var currentMove = minLossMoves.Value.First();
            minLossMoves.Value.Remove(currentMove);
            if (minLossMoves.Value.Count == 0)
            {
                availableMoves.Remove(minLossMoves.Key);
            }

            foreach (var possibleMove in GetPossibleMoves(currentMove, gridData, useUltra))
            {
                AddMove(availableMoves, possibleMove);

                if (possibleMove.EndPosition == endPos)
                {
                    // we have found the destination!
                    if (!useUltra || useUltra && possibleMove.StraightCount >= 4)
                    {
                        return possibleMove;
                    }
                }
            }
        }

        return null;
    }

    private IEnumerable<Move> GetPossibleMoves(Move currentMove, Dictionary<Vector2, PositionData> gridData, bool useUltra)
    {
        var directions = new List<Vector2>();
        var possibleMoves = new List<Move>();

        if (useUltra)
        {
            if (currentMove.StraightCount < 4)
            {
                var straightmoves = new List<Move>();
                // 3 moves straight
                while (currentMove.StraightCount < 4)
                {
                    var direction = currentMove.Direction;
                    var possibleMove = CreateMove(currentMove, direction);
                    if (possibleMove != null)
                    {
                        if (!IsBestLoss(possibleMove, gridData, useUltra))
                        {
                            // already better move at this position, don't continue from here
                            return new List<Move>();
                        }
                        straightmoves.Add(possibleMove);

                        // move to next position
                        currentMove = possibleMove;
                    }
                    else
                    {
                        // cannot move 4 times in a straight row, cannot continue from here
                        // return empty list
                        return new List<Move>();
                    }
                }
                // when we arrive here, we have moved 4 allowed straight moves
                foreach (var move in straightmoves)
                {
                    AddToGridData(move, gridData);
                }
            }

            // current move is now straight count >= 4
            // turn left and right
            directions.Add(MathUtils.TurnRight(currentMove.Direction));
            directions.Add(MathUtils.TurnLeft(currentMove.Direction));

            // if straight count < 10 also keep moving in the same direction
            if (currentMove.StraightCount < 10)
            {
                directions.Add(currentMove.Direction);
            }
        }
        else
        {
            // turn left and right
            directions.Add(MathUtils.TurnRight(currentMove.Direction));
            directions.Add(MathUtils.TurnLeft(currentMove.Direction));

            // if straight count < 3 also keep moving in the same direction
            if (currentMove.StraightCount < 3)
            {
                directions.Add(currentMove.Direction);
            }
        }

        foreach (var direction in directions)
        {
            var possibleMove = CreateMove(currentMove, direction);
            if (possibleMove != null)
            {
                if (IsBestLoss(possibleMove, gridData, useUltra))
                {
                    // best move for this position
                    possibleMoves.Add(possibleMove);
                    AddToGridData(possibleMove, gridData);
                }
            }
        }

        return possibleMoves;
    }

    private Move? CreateMove(Move currentMove, Vector2 direction)
    {
        var newPos = currentMove.EndPosition + direction;

        var straightCount = currentMove.Direction == direction ? currentMove.StraightCount + 1 : 1;

        if (Grid.TryGetValue(newPos, out var newLoss))
        {
            var possibleMove = new Move()
            {
                EndPosition = newPos,
                Direction = direction,
                StraightCount = straightCount,
                Loss = currentMove.Loss + newLoss
            };
            possibleMove.Route = new List<Vector2>(currentMove.Route);
            possibleMove.Route.Add(newPos);

            return possibleMove;
        }
        return null;
    }

    private bool IsBestLoss(Move currentMove, Dictionary<Vector2, PositionData> gridData, bool useUltra)
    {
        if (!gridData.TryGetValue(currentMove.EndPosition, out var tileData))
        {
            tileData = new PositionData();
            gridData[currentMove.EndPosition] = tileData;
        }

        if (!tileData.Moves.TryGetValue(currentMove.Direction, out var movesInDir))
        {
            // no data, this must be first and therefore best move
            return true;
        }

        if (!useUltra)
        {
            // we've reached this before from this direction with lower loss and lower or equal remaining stright steps
            if (movesInDir.Any(m => m.Loss <= currentMove.Loss && m.StraightCount <= currentMove.StraightCount))
            {
                // disregard this move
                return false;
            }
        }
        else
        {
            if (movesInDir.Any(m => m.Loss <= currentMove.Loss && m.StraightCount == currentMove.StraightCount))
            {
                // disregard this move
                return false;
            }
        }
        return true;
    }

    private void AddToGridData(Move move, Dictionary<Vector2, PositionData> gridData)
    {
        if (!gridData.TryGetValue(move.EndPosition, out var tileData))
        {
            tileData = new PositionData();
            gridData[move.EndPosition] = tileData;
        }

        if (!tileData.Moves.TryGetValue(move.Direction, out var movesInDir))
        {
            movesInDir = new List<Move>();
            tileData.Moves[move.Direction] = movesInDir;
        }
        movesInDir.Add(move);
    }


    private void AddMove(IDictionary<int, List<Move>> dictionary, Move move)
    {
        var loss = move.Loss;
        if (!dictionary.TryGetValue(loss, out var moves))
        {
            moves = new List<Move>();
            dictionary[loss] = moves;
        }
        moves.Add(move);
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
                    Direction = MathUtils.TurnRight(currentMove.Direction),
                    StraightCount = 1
                });
                directions.Add(new MoveDirection()
                {
                    Direction = MathUtils.TurnLeft(currentMove.Direction),
                    StraightCount = 1
                });
            }
        }
        else
        {
            directions.Add(new MoveDirection()
            {
                Direction = MathUtils.TurnRight(currentMove.Direction),
                StraightCount = 1
            });
            directions.Add(new MoveDirection()
            {
                Direction = MathUtils.TurnLeft(currentMove.Direction),
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

}
