namespace AoC2023.Days.Day11;

internal class Day11Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 11,
            Title = "Title",
            // Image,
            CreateViewModel = () => new Day11ViewModel(),
            ViewType = typeof(Day11Control)
        };
    }
}
