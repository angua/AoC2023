namespace AoC2023Lib.Days.Day20Lib;

internal class Button : Device
{
    public Button()
    {
        Name = "Button";
        OutputStrings.Add("broadcaster");
    }

    public override List<Signal> ProcessSignal(Signal inputSignal)
    {
        // push button sends low pulse
        var signals = new List<Signal>();

        foreach (var destination in Outputs)
        {
            signals.Add(new Signal()
            {
                Source = this,
                Destination = destination,
                Pulse = SignalType.Low,
                Cycle = inputSignal.Cycle
            });
        }

        return signals;
    }

    public override void Setup()
    { }
}
