namespace AoC2023Lib.Days.Day02Lib;

public class CubeGame
{
    private string _line;

    public int Id { get; private set; }

    public List<Dictionary<CubeColor, int>> Draws { get; private set; } = new();

    public Dictionary<CubeColor, int> MinimumSet { get; private set; } = new();

    public CubeGame(string line)
    {
        _line = line;

        // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green

        var parts = line.Split(':');

        // Game 1
        var gameparts = parts[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        Id = int.Parse(gameparts[1]);

        // 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
        var drawparts = parts[1].Split(";", StringSplitOptions.RemoveEmptyEntries);

        // 3 blue, 4 red
        foreach ( var drawpart in drawparts ) 
        {
            var drawdict = new Dictionary<CubeColor, int>()
            { {CubeColor.red, 0},
             {CubeColor.green, 0},
             {CubeColor.blue, 0},
            };
            var drawcolorparts = drawpart.Split(",", StringSplitOptions.RemoveEmptyEntries);

            // 3 blue
            foreach (var drawcolorpart in drawcolorparts )
            {
                var colorparts = drawcolorpart.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                drawdict[GetColor(colorparts[1])] = int.Parse(colorparts[0]);
            }
            Draws.Add(drawdict);
        }

        MinimumSet = GetMinimumSet();
    }

    private CubeColor GetColor(string colorString)
    {
        return colorString switch
        {
            "red" => CubeColor.red,
            "blue" => CubeColor.blue,
            "green" => CubeColor.green
        };
    }

    public Dictionary<CubeColor, int> GetMinimumSet()
    {
        var set = new Dictionary<CubeColor, int>();

        foreach (CubeColor color in Enum.GetValues(typeof(CubeColor)))
        {
            var maxdraw = Draws.Max(d => d[color]);
            set[color] = maxdraw;
        }
        return set;
    }
}
