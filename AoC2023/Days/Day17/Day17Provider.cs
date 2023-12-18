namespace AoC2023.Days.Day17;

internal class Day17Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 17,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day17ViewModel(),
            ViewType = typeof(Day17Control)
        };
    }
}
