using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2023Lib.Days.Day08Lib;

namespace AoC2023Lib.Days.Day24Lib;

public class HailStone
{
    private string line;

    public long PositionX { get; set; }
    public long PositionY { get; set; }
    public long PositionZ { get; set; }
    public long VelocityX { get; set; }
    public long VelocityY { get; set; }
    public long VelocityZ { get; set; }

    public HailStone(string line)
    {
        this.line = line;
        // 364193859817003, 337161998875178, 148850519939119 @ 85, 85, 473

        var parts = line.Split('@');

        var positionParts = parts[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
        PositionX = long.Parse(positionParts[0].Trim());
        PositionY = long.Parse(positionParts[1].Trim());
        PositionZ = long.Parse(positionParts[2].Trim());

        var velocityParts = parts[1].Split(",", StringSplitOptions.RemoveEmptyEntries);
        VelocityX = long.Parse(velocityParts[0].Trim());
        VelocityY = long.Parse(velocityParts[1].Trim());
        VelocityZ = long.Parse(velocityParts[2].Trim());
    }

    public HailStone(HailStone hailStone)
    {
        PositionX = hailStone.PositionX;
        PositionY = hailStone.PositionY;
        PositionZ = hailStone.PositionZ;
        VelocityX = hailStone.VelocityX;
        VelocityY = hailStone.VelocityY;
        VelocityZ = hailStone.VelocityZ;
    }
}
