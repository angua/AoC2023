namespace AoC2023.Days.Day14;

internal class Day14Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 14,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day14ViewModel(),
            ViewType = typeof(Day14Control)
        };
    }
}
