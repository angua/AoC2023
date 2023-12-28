namespace AoC2023Lib.Days.Day20Lib;

internal class OutputDevice : Device
{
    public bool HasReceivedLowPulse { get; set; } = false;



    public override List<Signal> ProcessSignal(Signal inputSignal)
    {
        if (inputSignal.Pulse == SignalType.Low) 
        {
            HasReceivedLowPulse = true;
        }
            return new List<Signal>();
    }

    public override void Setup()
    {
    }
}
