namespace AoC2023Lib.Days.Day19Lib;

public class Condition
{
    public Condition(string input)
    {
        // a<2006:qkq
        ConditionCategory = MachinePartSorter.GetCategory(input[0].ToString());
        Operator = input[1].ToString();
        Num = int.Parse(input.Substring(2));
    }

    public Category ConditionCategory { get; set; }
    public int Num {  get; set; }
    public string Operator { get; set; }

    public bool CheckCondition(PartRating rating)
    {
        return Operator switch
        {
            "<" => rating.Ratings[ConditionCategory] < Num,
            ">" => rating.Ratings[ConditionCategory] > Num,
            _ => throw new InvalidOperationException($"Unknown operator {Operator}")
        };
    }

    public ProcessedRange ProcessRange(PartRatingRange range)
    {
        var processed = new ProcessedRange();

        var rangeMin = range.RatingRanges[ConditionCategory].Item1;
        var rangeMax = range.RatingRanges[ConditionCategory].Item2;

        if (Operator == "<")
        {
            // 3 possibilities:
            // out of range:                |     [///////////]
            // overlapping             [////|////]
            // inside        [////////]     |

           // a < 2006
           if (rangeMin >= Num)
            {
                // outside, no matching, the whole range is unmatching
                processed.UnmatchingRange = new PartRatingRange(range);
            }
            else
            {
                if (rangeMax < Num)
                {
                    // inside, the whole range is matching
                    processed.MatchingRange = new PartRatingRange(range);
                }
                else
                {
                    // overlap
                    processed.MatchingRange = new PartRatingRange(range);
                    processed.MatchingRange.RatingRanges[ConditionCategory] = (rangeMin, Num - 1);

                    processed.UnmatchingRange = new PartRatingRange(range);
                    processed.UnmatchingRange.RatingRanges[ConditionCategory] = (Num, rangeMax);
                }
            }
        }
        else if (Operator == ">")
        {
            // 3 possibilities:
            // outside        [////////]     |
            // overlapping              [////|////]
            // inside:                       |     [///////////]

            // a > 2006
            if (rangeMax <= Num)
            {
                // outside, no matching, the whole range is unmatching
                processed.UnmatchingRange = new PartRatingRange(range);
            }
            else
            {
                if (rangeMin > Num)
                {
                    // inside, the whole range is matching
                    processed.MatchingRange = new PartRatingRange(range);
                }
                else
                {
                    // overlap
                    processed.MatchingRange = new PartRatingRange(range);
                    processed.MatchingRange.RatingRanges[ConditionCategory] = (Num + 1, rangeMax);

                    processed.UnmatchingRange = new PartRatingRange(range);
                    processed.UnmatchingRange.RatingRanges[ConditionCategory] = (rangeMin, Num);
                }
            }

        }

        return processed;
    }


}
