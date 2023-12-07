namespace AoC2023Lib.Days.Day07Lib;

public class Card
{
    public char CardChar { get; private set; }
    public int Value { get; private set; }
    public int ValueWithJokers { get; private set; }

    public Card(char card) 
    {
        CardChar = card;

        // A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2

        Value = card switch
        {
            'T' => 10,
            'J' => 11,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => int.Parse(card.ToString())
        };

        ValueWithJokers = card switch
        {
            'T' => 10,
            'J' => 1,
            'Q' => 12,
            'K' => 13,
            'A' => 14,
            _ => int.Parse(card.ToString())
        };

    }
}
