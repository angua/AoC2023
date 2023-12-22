namespace AoC2023.Days.Day19;

internal class Day19Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 19,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day19ViewModel(),
            ViewType = typeof(Day19Control)
        };
    }
}
