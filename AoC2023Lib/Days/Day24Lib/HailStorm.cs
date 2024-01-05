using System.Numerics;
using System.Security.Cryptography;
using Common;

namespace AoC2023Lib.Days.Day24Lib;

public class HailStorm
{
    public List<HailStone> HailStones { get; set; } = new();

    private long _testMin = 200000000000000;
    private long _testMax = 400000000000000;

    public long GetIntersectionCount()
    {
        var pairs = MathUtils.GetAllCombinations(HailStones.Count, 2);

        var intersectingPairs = new List<List<int>>();

        foreach (var pair in pairs)
        {
            var first = HailStones[pair[0]];
            var second = HailStones[pair[1]];

            if (IntersectWithinTestArea(first, second))
            {
                intersectingPairs.Add(pair);
            }
        }
        return intersectingPairs.Count;
    }

    private bool IntersectWithinTestArea(HailStone first, HailStone second)
    {
        // only use x and y coordinates
        // p1 + s v1 = p2 + t v2

        // x1 + s * vx1 = x2 + t * vx2     
        // y1 + s * vy1 = y2 + t * vy2

        // x1 - x2 + s * vx1 = t * vx2 
        // y1 - y2 + s * vy1 = t * vy2

        // (x1 - x2 + s * vx1) * vy2 = t * vx2 *  vy2
        // (y1 - y2 + s * vy1) * (-vx2) = - t * vx2 *  vy2

        // (x1 - x2 + s * vx1) * vy2 - (y1 - y2 + s * vy1) * vx2 = 0

        // x1 * vy2 - x2 * vy2 + s * vx1 * vy2 - y1 * vx2 + y2 * vx2 - s * vy1 * vx2 = 0

        // x1 * vy2 - x2 * vy2 - y1 * vx2 + y2 * vx2 = s (vy1 * vx2 - vx1 * vy2)

        // s = (x1 * vy2 - x2 * vy2 - y1 * vx2 + y2 * vx2) / (vy1 * vx2 - vx1 * vy2)

        var firstXYLine = new Line2D()
        {
            Position = new Vector2Double(first.PositionX, first.PositionY),
            Velocity = new Vector2Double(first.VelocityX, first.VelocityY)
        };
        var secondXYLine = new Line2D()
        {
            Position = new Vector2Double(second.PositionX, second.PositionY),
            Velocity = new Vector2Double(second.VelocityX, second.VelocityY)
        };

        var intersection = GetXYIntersection(firstXYLine, secondXYLine);

        if (intersection == null)
        {
            return false;
        }

        // intersection within test area
        if (intersection.X >= _testMin && intersection.X <= _testMax &&
            intersection.Y >= _testMin && intersection.Y <= _testMax)
        {
            return true;
        }

        return false;
    }


    public long GetStoneStartPositionSum()
    {
        Line2D? stonePathXY = null;
        Line3D? stonePath = null;

        // try with 5 hail stones
        var pairs = MathUtils.GetAllCombinations(6, 2);

        for (int x = -400; x < 400; x++)
        {
            if (stonePathXY != null)
            {
                break;
            }
            for (int y = -400; y < 400; y++)
            {
                if (stonePathXY != null)
                {
                    break;
                }

                var correctVelocity = true;
                Vector2Double? foundIntersectionXY = null;

                foreach (var pair in pairs)
                {
                    var first = HailStones[pair[0]];
                    var second = HailStones[pair[1]];

                    var firstXYLine = new Line2D()
                    {
                        Position = new Vector2Double(first.PositionX, first.PositionY),
                        Velocity = new Vector2Double(first.VelocityX - x, first.VelocityY - y)
                    };
                    var secondXYLine = new Line2D()
                    {
                        Position = new Vector2Double(second.PositionX, second.PositionY),
                        Velocity = new Vector2Double(second.VelocityX - x, second.VelocityY - y)
                    };
                    var intersectionXY = GetXYIntersection(firstXYLine, secondXYLine);

                    if (intersectionXY == null)
                    {
                        continue;
                    }

                    if (foundIntersectionXY != null)
                    {
                        var deltaX1 = Math.Abs(intersectionXY.X - foundIntersectionXY.X);
                        var deltaY = Math.Abs(intersectionXY.Y - foundIntersectionXY.Y);
                        if (deltaX1 > 0.4 || deltaY > 0.4)
                        {
                            correctVelocity = false;
                            break;
                        }
                    }

                    foundIntersectionXY = intersectionXY;

                }
                if (correctVelocity && foundIntersectionXY != null)
                {
                    stonePathXY = new Line2D()
                    {
                        Position = new Vector2Double(foundIntersectionXY.X, foundIntersectionXY.Y),
                        Velocity = new Vector2Double(x, y)
                    };
                    break;
                }

            }
        }


        for (int z = -400; z < 400; z++)
        {
            if (stonePath != null)
            {
                break;
            }

            var correctVelocity = true;
            Vector2Double? foundIntersectionXZ = null;

            foreach (var pair in pairs)
            {
                var first = HailStones[pair[0]];
                var second = HailStones[pair[1]];

                var firstXZLine = new Line2D()
                {
                    Position = new Vector2Double(first.PositionX, first.PositionZ),
                    Velocity = new Vector2Double(first.VelocityX - stonePathXY.Velocity.X, first.VelocityZ - z)
                };
                var secondXZLine = new Line2D()
                {
                    Position = new Vector2Double(second.PositionX, second.PositionZ),
                    Velocity = new Vector2Double(second.VelocityX - stonePathXY.Velocity.X, second.VelocityZ - z)
                };
                var intersectionXZ = GetXYIntersection(firstXZLine, secondXZLine);

                if (intersectionXZ == null)
                {
                    continue;
                }

                if (foundIntersectionXZ != null)
                {
                    var deltaZ = Math.Abs(intersectionXZ.Y - foundIntersectionXZ.Y);

                    if (deltaZ > 0.4)
                    {
                        correctVelocity = false;
                        break;
                    }
                }

                foundIntersectionXZ = intersectionXZ;
            }

            if (correctVelocity && foundIntersectionXZ != null)
            {
                stonePath = new Line3D()
                {
                    Position = new Vector3Double(stonePathXY.Position.X, stonePathXY.Position.Y, foundIntersectionXZ.Y),
                    Velocity = new Vector3Double(stonePathXY.Velocity.X, stonePathXY.Velocity.Y, z)
                };
            }



        }
        return (long)(Math.Round(stonePath.Position.X) + Math.Round(stonePath.Position.Y) + Math.Round(stonePath.Position.Z));
    }

    private Vector2Double? GetXYIntersection(Line2D first, Line2D second)
    {

        var divisor = (double)(first.Velocity.Y * second.Velocity.X
                        - first.Velocity.X * second.Velocity.Y);

        if (divisor == 0)
        {
            // parallel, do not intersect
            return null;
        }

        // intersection time for hailstone 1
        var t1 = (double)((first.Position.X * second.Velocity.Y
                    - second.Position.X * second.Velocity.Y
                    - first.Position.Y * second.Velocity.X
                    + second.Position.Y * second.Velocity.X) /
                    divisor);

        if (t1 < 0)
        {
            // happened in the past
            return null;
        }

        // intersection position
        var intersectionX = (double)(first.Position.X + t1 * first.Velocity.X);
        var intersectionY = (double)(first.Position.Y + t1 * first.Velocity.Y);

        // intersection time for hailstone 2
        var t2 = (double)(intersectionX - second.Position.X) / second.Velocity.X;
        if (t2 < 0)
        {
            // happened in the past
            return null;
        }

        return new Vector2Double(intersectionX, intersectionY);


    }


    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            HailStones.Add(new HailStone(line));
        }
    }

}
