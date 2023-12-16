namespace AoC2023.Days.Day16;

internal class Day16Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 16,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day16ViewModel(),
            ViewType = typeof(Day16Control)
        };
    }
}
