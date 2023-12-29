namespace AoC2023Lib.Days.Day20Lib;

public class Signal
{
    public Device Source { get; set; }
    public Device Destination { get; set; }
    public SignalType Pulse { get; set; }
    public long Cycle { get; set; }
}
