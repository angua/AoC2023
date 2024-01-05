using AoC2023Lib.Days.Day23Lib;
using Common;

namespace AoC2023Lib.Days.Day25Lib;

public class SnowMachine
{
    public Dictionary<string, MachinePart> Parts { get; set; } = new();

    public Dictionary<Connection, long> ConnectionCounts { get; set; } = new();

    public List<MachinePart> TestedParts { get; set; } = new();

    public HashSet<Connection> CutConnections { get; set; } = new();

    public long GetGroupSizeProduct()
    {
        while (Parts.Count > TestedParts.Count)
        {
            CreateNextConnectionPaths();

            var ordered = ConnectionCounts.OrderByDescending(p => p.Value).ToList();

            // stop when the 3. connection count is 50 % bigger than the 4.
            if (ordered[2].Value > 1.3 * ordered[3].Value)
            {
                break;
            }
        }

        var orderedParts = ConnectionCounts.OrderByDescending(p => p.Value).ToList();
        for (int i = 0; i < 3; i++)
        {
            CutConnections.Add(orderedParts[i].Key);
        }

        var part = Parts.First();
        CreateConnectionPaths(part.Value, true);

        var group1Count = part.Value.ConnectionPathsAfterCut.Count + 1;
        var group2Count = Parts.Count - group1Count;

        return group1Count * group2Count;

    }

    public MachinePart CreateNextConnectionPaths()
    {
        var part = Parts.First(p => !TestedParts.Contains(p.Value)).Value;

        CreateConnectionPaths(part);
        part.CountConnections();
        AddCounts(part);
        TestedParts.Add(part);

        return part;
    }

    private void AddCounts(MachinePart part)
    {
        foreach (var connectioncount in part.ConnectionCounts)
        {
            if (!ConnectionCounts.TryGetValue(connectioncount.Key, out var count))
            {
                ConnectionCounts[connectioncount.Key] = connectioncount.Value;
            }
            else
            {
                ConnectionCounts[connectioncount.Key] = count + connectioncount.Value;
            }
        }
    }

    private void CreateConnectionPaths(MachinePart startPart, bool avoidCutConnections = false)
    {
        var available = new Dictionary<long, Dictionary<MachinePart, ConnectionPath>>();

        // direct connections
        foreach (var connectedPart in startPart.ConnectedParts)
        {
            if (avoidCutConnections && CutConnections.Contains(connectedPart.Value))
            {
                continue;
            }
            var connectionpath = new ConnectionPath();
            connectionpath.MachineParts.Add(startPart);
            connectionpath.MachineParts.Add(connectedPart.Key);
            connectionpath.Connections.Add(connectedPart.Value);

            // <MachinePart, connectionpath>
            if (avoidCutConnections)
            {
                startPart.ConnectionPathsAfterCut.Add(connectedPart.Key, connectionpath);
            }
            else
            {
                startPart.ConnectionPaths.Add(connectedPart.Key, connectionpath);
            }
            AddConnectionPaths(available, connectedPart.Key, connectionpath);
        }

        while (available.Any(p => p.Value.Count > 0))
        {
            var minLength = available.Where(p => p.Value.Count > 0).Min(p => p.Key);
            var smallestConnections = available[minLength];
            // <MachinePart, connectionpath>
            var currentPart = smallestConnections.First();

            // <machinepart, connection>
            foreach (var neighbor in currentPart.Key.ConnectedParts)
            {
                if (avoidCutConnections && CutConnections.Contains(neighbor.Value))
                {
                    // skip cut connections
                    continue;
                }

                if (neighbor.Key == startPart)
                {
                    continue;
                }



                if ((!avoidCutConnections && !startPart.ConnectionPaths.ContainsKey(neighbor.Key)) ||
                    (avoidCutConnections && !startPart.ConnectionPathsAfterCut.ContainsKey(neighbor.Key)))
                {
                    var connectionpath = new ConnectionPath(currentPart.Value);
                    connectionpath.MachineParts.Add(neighbor.Key);
                    connectionpath.Connections.Add(neighbor.Value);

                    if (avoidCutConnections)
                    {
                        startPart.ConnectionPathsAfterCut.Add(neighbor.Key, connectionpath);
                    }
                    else
                    {
                        startPart.ConnectionPaths.Add(neighbor.Key, connectionpath);
                    }
                    AddConnectionPaths(available, neighbor.Key, connectionpath);
                }
            }

            smallestConnections.Remove(currentPart.Key);

        }

    }


    private void AddConnectionPaths(Dictionary<long, Dictionary<MachinePart, ConnectionPath>> dictionary,
        MachinePart part, ConnectionPath connectionpath)
    {
        var length = connectionpath.ConnectionLength;

        if (!dictionary.TryGetValue(connectionpath.ConnectionLength, out var connectionpaths))
        {
            connectionpaths = new Dictionary<MachinePart, ConnectionPath>();
            dictionary[connectionpath.ConnectionLength] = connectionpaths;
        }
        connectionpaths.Add(part, connectionpath);
    }


    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            var part = new MachinePart(line);
            Parts.Add(part.Name, part);
        }

        CreateConnections();
    }

    private void CreateConnections()
    {
        // add parts only mentioned at the right side to new collection
        var newParts = new Dictionary<string, MachinePart>();
        foreach (var part in Parts)
        {
            foreach (var connected in part.Value.ConnectedPartStrings)
            {
                if (!Parts.TryGetValue(connected, out var connectedPart) &&
                    !newParts.TryGetValue(connected, out connectedPart))
                {
                    connectedPart = new MachinePart()
                    {
                        Name = connected
                    };
                    // add to new collection
                    newParts.Add(connected, connectedPart);
                }

                var connection = new Connection(part.Value, connectedPart);


                // connect
                part.Value.ConnectedParts.Add(connectedPart, connection);
                connectedPart.ConnectedParts.Add(part.Value, connection);
            }
        }

        // add new collection to Parts
        foreach (var newPart in newParts)
        {
            Parts.Add(newPart.Key, newPart.Value);
        }
    }
}
