using System;
using System.Collections.ObjectModel;
using System.Linq;
using Advent2023.Utils;
using AoC2023Lib.Days.Day18Lib;

namespace AoC2023.Days.Day18;

public class Day18ViewModel
{
    public LavaLagoon Lagoon { get; } = new();

    public long LavaVolume { get; set; }
    public long RealLavaVolume { get; set; }

    public ObservableCollection<VisualEdge> Edges { get; set; } = new();

    private long _offsetX;
    private long _offsetY;

    public long MapWidth { get; set; }
    public long MapHeight { get; set; }

    public Day18ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day18", "input.txt");
        Lagoon.Parse(fileData);

        // Parse visual edges
        _offsetX = (Lagoon.RealMinX < 0) ? Math.Abs(Lagoon.RealMinX) : 0;
        _offsetY = (Lagoon.RealMinY < 0) ? Math.Abs(Lagoon.RealMinY) : 0;

        // scale 
        var scaleDivisor = 5000;
            //(Lagoon.Instructions.Min(i => i.DistanceFromColor) / 10);

        MapWidth = 20 + (_offsetX + Lagoon.RealMaxX) / scaleDivisor;
        MapHeight = 20 + (_offsetY + Lagoon.RealMaxY) / scaleDivisor;

        for (int i = 0; i < Lagoon.RealCorners.Count; i++)
        {
            var firstCorner = Lagoon.RealCorners[i];
            var secondCorner = i + 1 >= Lagoon.RealCorners.Count ? Lagoon.RealCorners[0] : Lagoon.RealCorners[i + 1];
            Edges.Add(new VisualEdge()
            {
                StartX = (long)((firstCorner.X + _offsetX )/ scaleDivisor) + 10,
                EndX = (long)((secondCorner.X +_offsetX) / scaleDivisor) + 10,
                StartY = (long)((firstCorner.Y +_offsetY)/ scaleDivisor) + 10,
                EndY = (long)((secondCorner.Y + _offsetY) / scaleDivisor) + 10,
            });
        }


        //LavaVolume = Lagoon.GetLavaVolume();
        RealLavaVolume = Lagoon.GetRealLavaVolume();
        
    }
}
