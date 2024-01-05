namespace AoC2023Lib.Days.Day25Lib;

public class Connection
{
    private MachinePart value;
    private MachinePart connectedPart;

    public Connection(MachinePart first, MachinePart second)
    {
        MachineParts = new MachinePart[] { first, second };
        ConnectionString = string.Join(" - ", MachineParts.Select(x => x.Name));
    }

    public MachinePart[] MachineParts { get; set; }

    public string ConnectionString { get; set; }
}
