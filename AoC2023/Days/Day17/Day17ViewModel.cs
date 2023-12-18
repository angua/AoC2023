using Advent2023.Utils;
using AoC2023Lib.Days.Day17Lib;

namespace AoC2023.Days.Day17;

public class Day17ViewModel
{
    public HeatLossControl Control { get; } = new();

    public int LowestHeatLoss { get; set; }

    public Day17ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day17", "input.txt");
        Control.Parse(fileData);

        LowestHeatLoss = Control.GetLowestHeatLoss();
    }
}
