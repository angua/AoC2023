namespace AoC2023.Days.Day06;

internal class Day06Provider : IDayProvider
{
    public Day GetDay()
    {
        return new Day
        {
            DayNumber = 6,
            Title = "Wait For It",
            // Image,
            CreateViewModel = () => new Day06ViewModel(),
            ViewType = typeof(Day06Control)
        };
    }
}
