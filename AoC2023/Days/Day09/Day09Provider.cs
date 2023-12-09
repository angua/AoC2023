namespace AoC2023.Days.Day09;

internal class Day09Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 9,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day09ViewModel(),
            ViewType = typeof(Day09Control)
        };
    }
}
