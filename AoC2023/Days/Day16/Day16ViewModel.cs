using Advent2023.Utils;
using AoC2023Lib.Days.Day16Lib;

namespace AoC2023.Days.Day16;

public class Day16ViewModel
{
    public ContraptionControl Control { get; } = new();

    public int EnergizedTilesCount { get; set; }
    public int BestEnergizedTilesCount { get; set; }

    public Day16ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day16", "input.txt");
        Control.Parse(fileData);

        EnergizedTilesCount = Control.GetEnergizedTilesCount();
        BestEnergizedTilesCount = Control.GetBestEnergizedTilesCount();

    }
}
