namespace AoC2023Lib.Days.Day20Lib;

internal class Broadcaster : Device
{
    public Broadcaster(string line)
    {
        Parse(line);
    }

    public override List<Signal> ProcessSignal(Signal inputSignal)
    {
        // broadcast incoming signals to outputs
        var signals = new List<Signal>();

        foreach (var destination in Outputs)
        {
            signals.Add(new Signal()
            {
                Source = this,
                Destination = destination,
                Pulse = inputSignal.Pulse,
                Cycle = inputSignal.Cycle,
            });
        }
        return signals;
    }

    public override void Setup()
    { }

}
