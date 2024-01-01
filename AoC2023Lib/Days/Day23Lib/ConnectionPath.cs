using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day23Lib
{
    internal class ConnectionPath
    {
        public Tile Start {  get; set; }
        public Tile End { get; set; }

        public Connection Connection { get; set; }
    }
}
