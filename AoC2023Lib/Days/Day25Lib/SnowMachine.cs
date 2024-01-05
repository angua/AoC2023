using Common;

namespace AoC2023Lib.Days.Day25Lib;

public class SnowMachine
{
    public Dictionary<string, MachinePart> Parts { get; set; } = new();

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
                // connect
                part.Value.ConnectedParts.Add(connectedPart);
                connectedPart.ConnectedParts.Add(part.Value);
            }
        }
       
        // add new collection to Parts
        foreach (var newPart in newParts)
        {
            Parts.Add(newPart.Key, newPart.Value);
        }
    }
}
