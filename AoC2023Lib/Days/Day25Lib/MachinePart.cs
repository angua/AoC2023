using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day25Lib;

public class MachinePart
{
    private string line;
    public string Name { get; set; }

    public List<string> ConnectedPartStrings { get; set; } = new();

    public HashSet<MachinePart> ConnectedParts { get; set; } = new();


    public MachinePart()
    {}

    public MachinePart(string line)
    {
        this.line = line;
        //ncx: ncb zdz zlz pcj

        var parts = line.Split(':');

        Name = parts[0].Trim();

        var connectedPartStrings = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var str in  connectedPartStrings)
        {
            ConnectedPartStrings.Add(str.Trim());
        }
    }
}
