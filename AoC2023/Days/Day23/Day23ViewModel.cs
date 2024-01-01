using Advent2023.Utils;
using AoC2023Lib.Days.Day23Lib;

namespace AoC2023.Days.Day23;

public class Day23ViewModel
{
    public PathFinding PathFinder { get; set; } = new();

    public long LongestPath {  get; set; }
    public long LongestPathWithoutSlopes {  get; set; }

    public Day23ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day23", "input.txt");
        PathFinder.Parse(fileData);

        //LongestPath = PathFinder.FindLongestPath();
        LongestPathWithoutSlopes = PathFinder.FindLongestPathWithoutSlopes();
    }
}
