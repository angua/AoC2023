using System.Runtime.Intrinsics.X86;

namespace AoC2023Lib.Days.Day07Lib;

public class Set
{
    private string _line;

    public Set(string line)
    {
        _line = line;

        // JJJJ8 619
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < parts[0].Length; i++)
        {
            var character = parts[0][i];
            Hand[i] = new Card(character);
        }

        Bid = int.Parse(parts[1]);

        Type = GetHandType(Hand);
        TypeWithJokers = GetHandType(Hand, true);


    }

    public Card[] Hand { get; private set; } = new Card[5];

    public int Bid { get; private set; }

    public HandType Type { get; private set; }

    public HandType TypeWithJokers { get; private set; }

    public int Rank { get; set; }

    public int RankWithJokers { get; set; }

    private HandType GetHandType(Card[] hand, bool useJokers = false)
    {
        // <Card value, count>
        var cardCounts = new Dictionary<int, int>();

        for (int i = 2; i < 15; i++)
        {
            var cardsWithThisValue = hand.Where(c => c.Value == i);
            cardCounts[i] = cardsWithThisValue.Count();
        }

        // joker J has card value 11

        if (cardCounts.Max(c => c.Value) == 5)
        {
            return HandType.FiveOfAKind;
        }

        else if (cardCounts.Max(c => c.Value) == 4)
        {
            var maxCard = cardCounts.First(c => c.Value == 4);

            // add joker
            if (useJokers && cardCounts[11] > 0)
            {
                return HandType.FiveOfAKind;
            }

            return HandType.FourOfAKind;
        }

        else if (cardCounts.Max(c => c.Value) == 3)
        {
            var maxCard = cardCounts.First(c => c.Value == 3);

            if (useJokers) 
            {
                if (cardCounts[11] == 3)
                {
                    // add JJJ to any other
                    if (cardCounts.Any(c => c.Value == 2))
                    {
                        return HandType.FiveOfAKind;
                    }
                    return HandType.FourOfAKind;
                }
                if (cardCounts[11] == 2)
                {
                    return HandType.FiveOfAKind;
                }
                else if (cardCounts[11] == 1)
                {
                    return HandType.FourOfAKind;
                }
            }

            if (cardCounts.Any(c => c.Value == 2))
            {
                return HandType.FullHouse;
            }
            return HandType.ThreeOfAKind;
        }

        // two pairs
        else if (cardCounts.Where(c => c.Value == 2).Count() == 2)
        {
            if (useJokers)
            {
                if (cardCounts[11] == 2)
                {
                    // one of the pairs is J
                    return HandType.FourOfAKind;
                }
                else if (cardCounts[11] == 1)
                {
                    // add J to one of the pairs to make 3 + 2
                    return HandType.FullHouse;
                }
            }

            return HandType.TwoPair;
        }

        // one pair
        else if (cardCounts.Where(c => c.Value == 2).Count() == 1)
        {
            if (useJokers && cardCounts[11] > 0)
            { 
                // add J to pair or pair of JJ to any other
                return HandType.ThreeOfAKind;
            }
            return HandType.OnePair;
        }

        // 5 different
        if (useJokers && cardCounts[11] == 1)
        {
            return HandType.OnePair;
        }
        return HandType.HighCard;
    }
}