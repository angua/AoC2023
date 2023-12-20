using Advent2023.Utils;
using AoC2023Lib.Days.Day18Lib;

namespace AoC2023.Days.Day18;

public class Day18ViewModel
{
    public LavaLagoon Lagoon { get; } = new();

    public int LavaVolume { get; set; }
    public Day18ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day18", "input.txt");
        Lagoon.Parse(fileData);

        LavaVolume = Lagoon.GetLavaVolume();
    }
}
