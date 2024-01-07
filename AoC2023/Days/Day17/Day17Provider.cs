using Advent2023.Utils;

namespace AoC2023.Days.Day17;

internal class Day17Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 17,
            Title = "Clumsy Crucible",
            Image = ResourceUtils.LoadBitmapFromResource("Day17", "heatmap.jpg"),
            CreateViewModel = () => new Day17ViewModel(),
            ViewType = typeof(Day17Control)
        };
    }
}
