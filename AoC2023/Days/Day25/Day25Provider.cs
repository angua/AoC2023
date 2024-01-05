namespace AoC2023.Days.Day25;

internal class Day25Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 25,
            Title = "Snowverload",
            // Image,
            CreateViewModel = () => new Day25ViewModel(),
            ViewType = typeof(Day25Control)
        };
    }
}
