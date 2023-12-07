using System.Collections.Generic;
using System.Net.Http.Headers;
using Common;

namespace AoC2023Lib.Days.Day07Lib;

public class GameMaster
{
    public List<Set> Sets { get; private set; } = new();

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            Sets.Add(new Set(line));
        }
    }

    public long GetTotalWinnings(bool useJokers = false)
    {
        List<Set> allOrderedSets = GetOrderedSets(useJokers);

        long totalWin = 0;
        for (int i = 0; i < allOrderedSets.Count; i++)
        {
            var rank = allOrderedSets.Count - i;
            if (useJokers)
            {
                allOrderedSets[i].RankWithJokers = allOrderedSets.Count - i;
            }
            else
            {
                allOrderedSets[i].Rank = allOrderedSets.Count - i;
            }
            totalWin += allOrderedSets[i].Bid * rank;
        }

        return totalWin;
    }

    public List<Set> GetOrderedSets(bool useJokers)
    {
        var allOrderedSets = new List<Set>();

        foreach (HandType type in Enum.GetValues(typeof(HandType)))
        {
            List<Set> currentSets;
            if (useJokers)
            {
                currentSets = Sets.Where(s => s.TypeWithJokers == type).ToList();
            }
            else
            {
                currentSets = Sets.Where(s => s.Type == type).ToList();
            }

            var pos = 0;

            var orderedSets = OrderSets(currentSets, pos, useJokers);

            allOrderedSets.AddRange(orderedSets);
        }

        return allOrderedSets;
    }

    private List<Set> OrderSets(List<Set> currentSets, int pos, bool useJokers)
    {
        if (pos == 4)
        {
            if (useJokers)
            {
                return currentSets.OrderByDescending(s => s.Hand[4].ValueWithJokers).ToList();
            }
            else
            {
                return currentSets.OrderByDescending(s => s.Hand[4].Value).ToList();
            }
        }

        var orderedSets = new List<Set>();

        for (int i = 14; i >= 1; i--)
        {
            var newSets = new List<Set>();
            if (useJokers)
            {
                newSets = currentSets.Where(s => s.Hand[pos].ValueWithJokers == i).ToList();
            }
            else
            {
                newSets = currentSets.Where(s => s.Hand[pos].Value == i).ToList();
            }

            orderedSets.AddRange(OrderSets(newSets, pos + 1, useJokers));
        }

        return orderedSets;
    }
}
