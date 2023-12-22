namespace AoC2023Lib.Days.Day19Lib;

public class Workflow
{
    private string _line;

    public Workflow(string line)
    {
        // x{a<2006:qkq,m>2090:A,rfg}
        _line = line;

        var parts = line.Split("{", StringSplitOptions.RemoveEmptyEntries);
        Name = parts[0];

        var rulesPart = parts[1].Replace("}", "");
        var stringParts = rulesPart.Split(",", StringSplitOptions.RemoveEmptyEntries);
        foreach (var stringPart in stringParts)
        {
            Rules.Add(new Rule(stringPart));
        }
    }

    public string Name { get; set; }

    public List<Rule> Rules { get; set; } = new();

    internal string Process(PartRating item)
    {
        foreach (var rule in Rules)
        {
            if (rule.CheckCondition(item))
            {
                return rule.Destination;
            };
        }
        return Rules.Last().Destination;
    }
}
