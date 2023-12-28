using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day20Lib;

public class EngineDepartment
{
    public List<Device> Devices { get; set; } = new();

    public Device StartButton { get; set; }

    public void Parse(Filedata fileData)
    {
        StartButton = new Button();
        Devices.Add(StartButton);

        // %sx -> lq, fn
        foreach (var line in fileData.Lines)
        {
            var parts = line.Split('-', StringSplitOptions.RemoveEmptyEntries);

            var name = parts[0];

            if (name.Contains("broadcaster"))
            {
                Devices.Add(new Broadcaster(line));
            }
            else if (name.StartsWith('%'))
            {
                Devices.Add(new FlipFlop(line));
            }
            else if (name.StartsWith('&'))
            {
                Devices.Add(new Conjunction(line));
            }
        }

        ConnectDevices();
        foreach (var device in Devices)
        {
            device.Setup();
        }
    }

    private void ConnectDevices()
    {
        var additionalDevices = new List<Device>();
        foreach (var device in Devices)
        {
            foreach (var str in device.OutputStrings)
            {
                var destinationDevice = Devices.FirstOrDefault(d => d.Name == str);

                if (destinationDevice == null)
                {
                    // device with no outputs
                    destinationDevice = new OutputDevice()
                    {
                        Name = str
                    };

                    additionalDevices.Add(destinationDevice);
                }

                device.Outputs.Add(destinationDevice);
                destinationDevice.Inputs.Add(device);

            }
        }
        Devices.AddRange(additionalDevices);
    }


    public long GetPulseProduct()
    {
        long lowCount = 0;
        long highCount = 0;

        for (int cycle = 0; cycle < 1000; cycle++)
        {
            var (additionalLow, additionalHigh) = RunCycle();
            lowCount += additionalLow;
            highCount += additionalHigh;
        }

        return lowCount * highCount;
    }

  
    public (long, long) RunCycle()
    {
        var signalQueue = new Queue<Signal>();

        long lowSignalCount = 0;
        long highSignalCount = 0;

        // start by pushing button (adding input signal to button)
        var startSignal = StartButton.ProcessSignal(new Signal());

        (lowSignalCount, highSignalCount) = CountPulses(startSignal, lowSignalCount, highSignalCount);

        foreach (var signal in startSignal)
        {
            signalQueue.Enqueue(signal);
        }

        while (signalQueue.Count > 0)
        {
            // next signal step
            // let devices do their work
            var nextSignal = signalQueue.Dequeue();
            var outputSignals = nextSignal.Destination.ProcessSignal(nextSignal);

            // count signals
            (lowSignalCount, highSignalCount) = CountPulses(outputSignals, lowSignalCount, highSignalCount);

            foreach (var signal in outputSignals)
            {
                signalQueue.Enqueue(signal);
            }

        }

        return (lowSignalCount, highSignalCount);
    }

    private (long lowSignalCount, long highSignalCount) CountPulses(List<Signal> signals, long lowSignalCount, long highSignalCount)
    {
        var lowSignals = signals.Count(s => s.Pulse == SignalType.Low);
        lowSignalCount += lowSignals;

        highSignalCount += (signals.Count - lowSignals);

        return (lowSignalCount, highSignalCount);
    }

   
}
