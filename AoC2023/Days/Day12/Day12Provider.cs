namespace AoC2023.Days.Day12;

internal class Day12Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 12,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day12ViewModel(),
            ViewType = typeof(Day12Control)
        };
    }
}
