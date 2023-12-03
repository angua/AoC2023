namespace AoC2023.Days.Day03;

internal class Day03Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 3,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day03ViewModel(),
            ViewType = typeof(Day03Control)
        };
    }
}
