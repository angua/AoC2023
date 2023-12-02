namespace AoC2023Lib.Days.Day02Lib;

public class CubeGame
{
    private string _line;

    public int Id { get; private set; }

    public List<Dictionary<Color, int>> Draws { get; private set; } = new();

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
            var drawdict = new Dictionary<Color, int>()
            { {Color.red, 0},
             {Color.green, 0},
             {Color.blue, 0},
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


        

    }

    private Color GetColor(string colorString)
    {
        return colorString switch
        {
            "red" => Color.red,
            "blue" => Color.blue,
            "green" => Color.green
        };
    }

    public Dictionary<Color, int> GetMinimumSet()
    {
        var set = new Dictionary<Color, int>();

        foreach (Color color in Enum.GetValues(typeof(Color)))
        {
            var maxdraw = Draws.Max(d => d[color]);
            set[color] = maxdraw;
        }
        return set;
    }
}
