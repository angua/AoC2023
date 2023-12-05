namespace AoC2023Lib.Days.Day05Lib;

public class Mapping
{
    public Resource SourceResource { get; set; }
    public Resource DestinationResource { get; set; }

    public long SourceStart { get; set; }
    public long DestinationStart { get; set; }

    public long Range {  get; set; }
}
