namespace AoC2023Lib.Days.Day20Lib;

internal class FlipFlop : Device
{
    // start off
    private bool _isOn = false;

    public FlipFlop(string line)
    {
        Parse(line);
    }

    public override List<Signal> ProcessSignal(Signal inputSignal)
    {
        var signals = new List<Signal>();
        // ignore high, swap on low and send pulse
        if (inputSignal.Pulse == SignalType.Low)
        {
            if (_isOn == false)
            {
                _isOn = true;
                foreach (var destination in Outputs)
                {
                    signals.Add(new Signal()
                    {
                        Source = this,
                        Destination = destination,
                        Pulse = SignalType.High,
                        Cycle = inputSignal.Cycle
                    });
                }
            }
            else
            {
                _isOn = false;
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
            }
        }

        return signals;
    }

    public override void Setup()
    { }
}
