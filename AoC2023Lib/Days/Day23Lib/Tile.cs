﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day23Lib
{
    internal class Tile
    {
        public char Ground;

        public Vector2 Position { get; set; }

        public List<Tile> Neighbors { get; set; } = new();

        public Tile(char input)
        {
            Ground = input;
        }
    }
}
