using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2023Lib.Days.Day07Lib;

namespace AoC2023.Days.Day07;

public class VisualSet
{
    private Set _set;

    public string Hand {  get; set; }

    public string SortedHand { get; set; }

    public int Bid => _set.Bid;

    public HandType Type => _set.Type;
    public HandType TypeWithJokers => _set.TypeWithJokers;

    public int Rank => _set.Rank;
    public int RankWithJokers => _set.RankWithJokers;

    public VisualSet(Set set)
    {
        _set = set;
        Hand = string.Join("", _set.Hand.Select(c => c.CardChar));
        SortedHand = string.Join("", _set.Hand.Select(c => c.CardChar).Order());
    }
}
