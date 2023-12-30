namespace AoC2023.Days.Day22;

internal class Day22Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 22,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day22ViewModel(),
            ViewType = typeof(Day22Control)
        };
    }
}
