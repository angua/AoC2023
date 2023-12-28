namespace AoC2023Lib.Days.Day20Lib;

public abstract class Device
{
    public string Name { get; set; }

    public List<string> OutputStrings { get; set; } = new();

    public List<Device> Inputs { get; set; } = new();
    public List<Device> Outputs { get; set; } = new();

    public List<Signal> InputSignals { get; set; } = new();

    public void Parse(string line)
    {
        // broadcaster -> xr, mr, rg, sv

        var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);

        var nameStr = parts[0];
        nameStr = nameStr.Replace("%", string.Empty);
        nameStr = nameStr.Replace("&", string.Empty);
        Name = nameStr.Trim();

        var outputStr = parts[1].Replace(">", string.Empty);
        var outputParts = outputStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var part in outputParts)
        {
            OutputStrings.Add(part.Trim());
        }
    }

    public abstract void Setup();

    public abstract List<Signal> ProcessSignal(Signal inputSignal);

}
