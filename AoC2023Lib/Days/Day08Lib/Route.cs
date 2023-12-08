namespace AoC2023Lib.Days.Day08Lib
{
    internal class Route
    {
        public Position StartPosition { get; set; }

        // steps where a target position was reached (ends with Z)
        // (sequence num, step within sequence)
        public List<(int, int)> TargetSteps { get; set; } = new();

        // instruction sequence where the first target occurs
        public int FirstTargetSequence { get; set; }

        // after the first, targets start recurring every fixed number of sequences
        public int TargetSequenceDiff { get; set; }
    }
}