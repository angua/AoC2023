namespace AoC2023.Days.Day18;

internal class Day18Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 18,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day18ViewModel(),
            ViewType = typeof(Day18Control)
        };
    }
}
