namespace AoC2023.Days.Day08;

internal class Day08Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 8,
            Title = "Haunted Wasteland",
            // Image,
            CreateViewModel = () => new Day08ViewModel(),
            ViewType = typeof(Day08Control)
        };
    }
}
