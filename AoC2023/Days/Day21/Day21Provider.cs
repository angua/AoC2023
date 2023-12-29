namespace AoC2023.Days.Day21;

internal class Day21Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 21,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day21ViewModel(),
            ViewType = typeof(Day21Control)
        };
    }
}
