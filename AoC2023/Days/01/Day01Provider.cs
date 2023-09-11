using AoC2023.Days;
using AoC2023.Days.Day01;

namespace Days.Day01;

internal class Day01Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 1,
            Title = "Day 1 Title",
            // Image = ResourceUtils.LoadBitmapFromResource("Day01", "day01.jpg"),
            CreateViewModel = () => new Day01ViewModel(),
            ViewType = typeof(Day01Control)
        };
    }
}
