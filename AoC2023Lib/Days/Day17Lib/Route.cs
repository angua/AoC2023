using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day17Lib;

internal class Route
{
    public int Loss { get; set; }

    public List<RoutePosition> Positions { get; set; }

    public int StraightCount { get; set; }
}
