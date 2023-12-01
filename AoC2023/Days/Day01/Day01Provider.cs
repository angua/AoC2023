using Advent2023.Utils;

namespace AoC2023.Days.Day01;

internal class Day01Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 1,
            Title = "Trebuchet",
            Image = ResourceUtils.LoadBitmapFromResource("Day01", "calibrationinput.jpg"),
            CreateViewModel = () => new Day01ViewModel(),
            ViewType = typeof(Day01Control)
        };
    }
}
