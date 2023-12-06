namespace AoC2023.Days.Day05;

internal class Day05Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 5,
            Title = "If You Give A Seed A Fertilizer",
            // Image,
            CreateViewModel = () => new Day05ViewModel(),
            ViewType = typeof(Day05Control)
        };
    }
}
