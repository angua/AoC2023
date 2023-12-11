namespace AoC2023.Days.Day02;

internal class Day02Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 2,
            Title = "Cube Conundrum",
            // Image,
            CreateViewModel = () => new Day02ViewModel(),
            ViewType = typeof(Day02Control)
        };
    }
}
