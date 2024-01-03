namespace AoC2023.Days.Day24;

internal class Day24Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 24,
            Title = "Never Tell Me The Odds",
            // Image,
            CreateViewModel = () => new Day24ViewModel(),
            ViewType = typeof(Day24Control)
        };
    }
}
