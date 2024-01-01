namespace AoC2023.Days.Day23;

internal class Day23Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 23,
            Title = "A Long Walk",
            // Image,
            CreateViewModel = () => new Day23ViewModel(),
            ViewType = typeof(Day23Control)
        };
    }
}
