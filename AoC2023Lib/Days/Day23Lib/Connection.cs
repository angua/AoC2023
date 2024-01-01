using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day23Lib
{
    internal class Connection
    {
        public Route Path { get; set; }

        public List<Tile> Crossroads { get; set; } = new List<Tile>();

        public int StepCount { get; set; }
    }
}
