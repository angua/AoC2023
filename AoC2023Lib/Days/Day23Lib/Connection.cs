using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day23Lib
{
    public class Connection
    {
        public Route Path { get; set; }

        public Tile[] Crossroads { get; set; } = new Tile[2];

        public int StepCount { get; set; }
    }
}
