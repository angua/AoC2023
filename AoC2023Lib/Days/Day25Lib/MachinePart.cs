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

    public Dictionary<MachinePart, Connection> ConnectedParts { get; set; } = new();

    public Dictionary<MachinePart, ConnectionPath> ConnectionPaths { get; set; } = new();

    public Dictionary<Connection, long> ConnectionCounts { get; set; } = new();


    public Dictionary<MachinePart, ConnectionPath> ConnectionPathsAfterCut { get; set; } = new();


    public MachinePart()
    { }

    public MachinePart(string line)
    {
        this.line = line;
        //ncx: ncb zdz zlz pcj

        var parts = line.Split(':');

        Name = parts[0].Trim();

        var connectedPartStrings = parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var str in connectedPartStrings)
        {
            ConnectedPartStrings.Add(str.Trim());
        }
    }

    internal void CountConnections()
    {
        // <machinepart, connectionpath>
        foreach (var path in ConnectionPaths)
        {
            foreach (var connection in path.Value.Connections)
            {
                if (!ConnectionCounts.TryGetValue(connection, out var count))
                {
                    ConnectionCounts[connection] = 1;
                }
                else
                {
                    count++;
                }
            }
        }
    }
}
