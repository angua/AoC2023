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

        var divisor = (double)(first.VelocityY * second.VelocityX
                        - first.VelocityX * second.VelocityY);

        if (divisor == 0)
        {
            // parallel, do not intersect
            return false;
        }

        // intersection time for hailstone 1
        var t1 = (double)((first.PositionX * second.VelocityY
                    - second.PositionX * second.VelocityY
                    - first.PositionY * second.VelocityX
                    + second.PositionY * second.VelocityX) /
                    divisor);
                    
        if (t1 < 0)
        {
            // happened in the past
            return false;
        }

        // intersection position
        var intersectionX = (double)(first.PositionX + t1 * first.VelocityX);
        var intersectionY = (double)(first.PositionY + t1 * first.VelocityY);

        // intersection time for hailstone 2
        var t2 = (double)(intersectionX - second.PositionX) / second.VelocityX;
        if (t2 < 0)
        {
            // happened in the past
            return false;
        }

        // intersection within test area
        if (intersectionX >= _testMin && intersectionX <= _testMax &&
            intersectionY >= _testMin && intersectionY <= _testMax)
        {
            return true;
        }

        return false;

    }

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            HailStones.Add(new HailStone(line));
        }
    }


}
