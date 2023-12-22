namespace AoC2023Lib.Days.Day19Lib;

public class PartRating
{
    private string _line;

    public PartRating(string line)
    {
        _line = line;

        // x = 787,m = 2655,a = 1222,s = 2876
        var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in parts )
        {
            var entryParts = part.Split('=', StringSplitOptions.RemoveEmptyEntries);
            var category = MachinePartSorter.GetCategory(entryParts[0].Trim());
            var num = int.Parse(entryParts[1].Trim());

            Ratings.Add(category, num);
        }
        RatingSum = Ratings.Sum(r => r.Value);
    }

    public Dictionary<Category, int> Ratings { get; set; } = new();
    public int RatingSum { get; internal set; }
}
