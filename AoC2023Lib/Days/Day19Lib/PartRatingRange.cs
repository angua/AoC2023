namespace AoC2023Lib.Days.Day19Lib;

public class PartRatingRange
{
    public PartRatingRange()
    { }

    public PartRatingRange(PartRatingRange range)
    {
        foreach ( var item in range.RatingRanges)
        {
            RatingRanges.Add(item.Key, item.Value);
        }
    }

    // (first number, last number) in range
    public Dictionary<Category, (int, int)> RatingRanges { get; set; } = new();


    public ulong Possiblities { get; set; }

    public ulong GetPossibilities()
    {
        ulong product = 1;

        foreach (var item in RatingRanges)
        {
            ulong diff = (ulong)item.Value.Item2 - (ulong)item.Value.Item1;
            product *= (diff + 1);
        }
        Possiblities = product;
        return product;
    }

}
