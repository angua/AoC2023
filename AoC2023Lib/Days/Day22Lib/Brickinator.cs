using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day22Lib;

public class Brickinator
{
    private List<Brick> _bricks = new();

    private List<Brick> _sortedBricks = new();

    // position, brick id
    private Dictionary<Vector3, int> _brickPositions = new();

    private List<Brick> _bricksToDisintegrate = new();



    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            var brick = new Brick(line);
            brick.Id = _bricks.Count;
            _bricks.Add(brick);
        }

        _sortedBricks = _bricks.OrderBy(b => b.StartPos1.Z).ToList();

        DropBricks();
        ConnectBricks();
    }


    private void DropBricks()
    {
        var maxHeight = 1;
        var minHeight = 1;

        foreach (var brick in _sortedBricks)
        {
            // bottom area
            var zBottom = brick.StartPositions.Min(p => p.Z);
            var bottom = brick.StartPositions.Where(p => p.Z == zBottom);

            var endZ = maxHeight + 1;
            var heightoccupied = false;

            // drop down, start above max stack height
            for (var testZ = maxHeight; testZ >= minHeight; testZ--)
            {
                if (heightoccupied)
                {
                    break;
                }

                foreach (var bottomPos in bottom)
                {
                    var testPos = new Vector3(bottomPos.X, bottomPos.Y, testZ);
                    if (_brickPositions.ContainsKey(testPos))
                    {
                        // already occupied, can not place here
                        heightoccupied = true;
                        break;
                    }
                }
                if (!heightoccupied)
                {
                    // possible position
                    endZ = testZ;
                }
            }

            // place at final height
            var deltaZ = zBottom - endZ;
            foreach (var pos in brick.StartPositions)
            {
                var newPos = new Vector3(pos.X, pos.Y, pos.Z - deltaZ);
                _brickPositions[newPos] = brick.Id;
                brick.Endpositions.Add(newPos);
            }
            // update max stack height
            var brickMaxZ = brick.Endpositions.Max(p => p.Z);

            maxHeight = (int)Math.Max(maxHeight, brickMaxZ);
        }
    }

    private void ConnectBricks()
    {
        foreach (var brick in _sortedBricks)
        {
            // look below
            // bottom area
            var zBottom = brick.Endpositions.Min(p => p.Z);

            if (zBottom > 1)
            {
                var bottom = brick.Endpositions.Where(p => p.Z == zBottom);

                foreach (var bottomPos in bottom)
                {
                    var testPos = new Vector3(bottomPos.X, bottomPos.Y, bottomPos.Z - 1);
                    if (_brickPositions.TryGetValue(testPos, out var otherBrickId))
                    {
                        var otherBrick = _bricks.First(b => b.Id == otherBrickId);

                        brick.BricksBelow.Add(otherBrick);
                        otherBrick.BricksAbove.Add(brick);
                    }
                }

            }

        }
    }

    public long GetDisintegrateSum()
    {
        foreach (var brick in _sortedBricks)
        {
            if (brick.BricksAbove.Count == 0)
            {
                _bricksToDisintegrate.Add(brick);
            }
            else
            {
                var canDisintegrate = true;
                foreach (var brickAbove in brick.BricksAbove)
                {
                    if (brickAbove.BricksBelow.Count == 1)
                    {
                        canDisintegrate = false;
                    }
                }
                if (canDisintegrate)
                {
                    _bricksToDisintegrate.Add(brick);
                }
            }
        }
        return _bricksToDisintegrate.Count;
    }

    public long GetFallingSum()
    {
        var fallingBrickSum = 0;

        var bricksWithFalling = _bricks.Except(_bricksToDisintegrate);

        foreach (var brick in bricksWithFalling)
        {
            var fallingBricks = new HashSet<Brick>();
            var bricksToInvestigate = new Queue<Brick>();

            bricksToInvestigate.Enqueue(brick);

            while (bricksToInvestigate.Count > 0)
            {
                var testBrick = bricksToInvestigate.Dequeue();

                var bricksAbove = testBrick.BricksAbove;

                foreach (var brickAbove in bricksAbove)
                {
                    var falling = true;
                    var bricksBelowBrickAbove = brickAbove.BricksBelow;

                    foreach (var brickBelow in bricksBelowBrickAbove)
                    {
                        if (brickBelow != brick && !fallingBricks.Contains(brickBelow))
                        {
                            falling = false;
                            break;
                        }
                    }
                    if (falling)
                    {
                        fallingBricks.Add(brickAbove);
                        bricksToInvestigate.Enqueue(brickAbove);
                    }

                }
            }

            fallingBrickSum += fallingBricks.Count;
        }
        return fallingBrickSum;
    }

}
