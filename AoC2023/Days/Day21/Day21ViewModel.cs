using Advent2023.Utils;
using AoC2023Lib.Days.Day21Lib;

namespace AoC2023.Days.Day21;

public class Day21ViewModel
{
    public WalkingElf Walker { get; set; } = new();

    public long Plots64Steps { get; set; }
    public long Plots26MSteps { get; set; }
    public Day21ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day21", "input.txt");
        Walker.Parse(fileData);

        Plots64Steps = Walker.GetPlots64Steps();
        Plots26MSteps = Walker.GetPlots26MSteps();
    }
}
