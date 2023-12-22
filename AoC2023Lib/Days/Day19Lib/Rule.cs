using System.Diagnostics;

namespace AoC2023Lib.Days.Day19Lib;

public class Rule
{
    private string _ruleString;

    public Rule(string input)
    {
        // x{a<2006:qkq,m>2090:A,rfg}

        _ruleString = input;

        var parts = _ruleString.Split(":");

        if (parts.Length == 1)
        {
            // no condition (last rule)
            Destination = parts[0];
        }
        else
        {
            RuleCondition = new Condition(parts[0]);
            Destination = parts[1];
        }
    }

    public Condition? RuleCondition { get; set; } = null;
    
    public string Destination { get; set; }

    internal bool CheckCondition(PartRating item)
    {
        return RuleCondition == null || RuleCondition.CheckCondition(item);
    }
}
