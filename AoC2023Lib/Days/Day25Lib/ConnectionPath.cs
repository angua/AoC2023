namespace AoC2023Lib.Days.Day25Lib;

public class ConnectionPath
{
    public ConnectionPath()
    { }

    public ConnectionPath(ConnectionPath currentPath)
    {
        Connections = new List<Connection>(currentPath.Connections);

        foreach (var part in currentPath.MachineParts)
        {
            MachineParts.Add(part);
        }
    }

    public List<Connection> Connections { get; set; } = new();
    public HashSet<MachinePart> MachineParts { get; set; } = new();

    public long ConnectionLength => Connections.Count;

}
