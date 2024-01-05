using Advent2023.Utils;
using AoC2023Lib.Days.Day24Lib;

namespace AoC2023.Days.Day24;

public class Day24ViewModel
{
    public HailStorm Storm { get; } = new();

    public long IntersectionCount { get; set; }
    public long StoneStartPositionSum { get; set; }
    public Day24ViewModel()
    {
        var fileData = ResourceUtils.LoadDataFromResource("Day24", "input.txt");
        Storm.Parse(fileData);

        IntersectionCount = Storm.GetIntersectionCount();
        StoneStartPositionSum = Storm.GetStoneStartPositionSum();
    }
}
