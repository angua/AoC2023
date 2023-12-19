using System.Collections.ObjectModel;
using Advent2023.Utils;
using AoC2023Lib.Days.Day17Lib;

namespace AoC2023.Days.Day17;

public class Day17ViewModel
{
    public HeatLossControl Control { get; } = new();

    public ObservableCollection<object> HeatLossMap { get; set; } = new();

    public int MapWidth => Control.CountX;
    public int MapHeight => Control.CountY;

    public int LowestHeatLoss { get; set; }
    public int LowestUltraHeatLoss { get; set; }

    public Day17ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day17", "input.txt");
        Control.Parse(fileData);
        
        //LowestHeatLoss = Control.GetLowestHeatLoss();
        LowestUltraHeatLoss = Control.GetLowestUltraHeatLoss();

        /*
        foreach (var pos in Control.Grid)
        {
            HeatLossMap.Add(new VisualPosition()
            {
                PositionX = (int)pos.Key.X,
                PositionY = (int)pos.Key.Y,
                Loss = pos.Value
            });
        }
        */
    }
}
