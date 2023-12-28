namespace AoC2023.Days.Day15;

internal class Day15Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 15,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day15ViewModel(),
            ViewType = typeof(Day15Control)
        };
    }
}
