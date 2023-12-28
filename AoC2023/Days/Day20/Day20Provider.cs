namespace AoC2023.Days.Day20;

internal class Day20Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 20,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day20ViewModel(),
            ViewType = typeof(Day20Control)
        };
    }
}
