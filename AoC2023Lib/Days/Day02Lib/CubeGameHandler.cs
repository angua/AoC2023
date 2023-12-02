using Common;

namespace AoC2023Lib.Days.Day02Lib;

public class CubeGameHandler
{
    public List<CubeGame> CubeGames { get; set; } = new();

    public List<CubeGame> GetPossibleDraws(Dictionary<Color, int> totalSet)
    {
        var result = new List<CubeGame>();

        foreach (var game in CubeGames)
        {
            var valid = true;
            foreach (var draw in game.Draws)
            {
                if ( !valid )
                {
                    break;
                }
                
                foreach (var colorDraw in draw)
                {
                    if (colorDraw.Value > totalSet[colorDraw.Key])
                    {
                        valid = false;
                        break;
                    }
                }
            }
            if (valid)
            {
                result.Add(game);
            }
        }
        return result;
    }

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            CubeGames.Add(new CubeGame(line));
        }

    }
}
