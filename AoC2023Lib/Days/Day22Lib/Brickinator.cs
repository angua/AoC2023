using System.Numerics;
using Common;

namespace AoC2023Lib.Days.Day22Lib;

public class Brickinator
{
    private List<Brick> _bricks = new();

    private List<Brick> _sortedBricks = new();

    // position, brick id
    private Dictionary<Vector3, int> _brickPositions = new();

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
                        brick.BricksBelow.Add(otherBrickId);

                        var otherBrick = _bricks.First(b => b.Id == otherBrickId);
                        otherBrick.BricksAbove.Add(brick.Id);
                    }
                }

            }

        }
    }

    public long GetDisintegrateSum()
    {
        var bricksToDisintegrate = new List<Brick>();
        foreach ( var brick in _sortedBricks)
        {
            if (brick.BricksAbove.Count == 0)
            {
                bricksToDisintegrate.Add(brick);
            }
            else
            {
                var canDisintegtrate = true;
                foreach (var brickAboveId in brick.BricksAbove)
                {
                    var brickAbove = _bricks.First(b => b.Id == brickAboveId);
                    if (brickAbove.BricksBelow.Count == 1)
                    {
                        canDisintegtrate = false;
                    }
                }
                if (canDisintegtrate)
                {
                    bricksToDisintegrate.Add(brick);
                }
            }
        }
        return bricksToDisintegrate.Count;
    }
}
