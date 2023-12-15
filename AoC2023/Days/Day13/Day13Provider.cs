namespace AoC2023.Days.Day13;

internal class Day13Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 13,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day13ViewModel(),
            ViewType = typeof(Day13Control)
        };
    }
}
