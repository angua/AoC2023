using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day24Lib
{
    public class Vector2Double
    {
        public Vector2Double()
        {
        }

        public Vector2Double(double x, double y)
        {
            X = x; 
            Y = y;
        }

        public double X {  get; set; }
        public double Y { get; set; }
    }
}
