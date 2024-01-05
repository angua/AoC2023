using Advent2023.Utils;
using AoC2023Lib.Days.Day25Lib;

namespace AoC2023.Days.Day25;

public class Day25ViewModel
{
    public SnowMachine Machine { get; } = new();
    public Day25ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day25", "input.txt");
        Machine.Parse(fileData);
    }
}
