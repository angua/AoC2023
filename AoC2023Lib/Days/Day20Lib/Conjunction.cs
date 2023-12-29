namespace AoC2023Lib.Days.Day20Lib;

internal class Conjunction : Device
{
    // low false, high true
    private Dictionary<Device, bool> _inputStates = new();


    public Conjunction(string line)
    {
        Parse(line);
    }

    public override List<Signal> ProcessSignal(Signal inputSignal)
    {
        var signals = new List<Signal>();

        if (Tracking)
        {
            SignalHistory.Add(inputSignal);
        }

        // update input state
        if (inputSignal.Pulse == SignalType.Low)
        {
            _inputStates[inputSignal.Source] = false;
        }
        else
        {
            _inputStates[inputSignal.Source] = true;
        }

        // if it remembers high pulses for all inputs, it sends a low pulse;
        // otherwise, it sends a high pulse.
        var outputPulse = _inputStates.Any(p => p.Value == false) ? SignalType.High : SignalType.Low;

        foreach (var destination in Outputs)
        {
            signals.Add(new Signal()
            {
                Source = this,
                Destination = destination,
                Pulse = outputPulse,
                Cycle = inputSignal.Cycle,
            });
        }
        return signals;
    }

    public override void Setup()
    {
        foreach (var inputdevice in Inputs)
        {
            // all inputs start low
            _inputStates[inputdevice] = false;
        }
    }
}
