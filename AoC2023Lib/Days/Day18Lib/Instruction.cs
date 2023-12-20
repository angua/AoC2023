namespace AoC2023Lib.Days.Day18Lib;

public class Instruction
{
    private string _line;

    public Instruction(string line)
    {
        _line = line;

        // R 6 (#70c710)
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Direction = parts[0];
        NumberOfHoles = int.Parse(parts[1]);
        Color = parts[2];
    }

    public string Direction { get; set; }
    public int NumberOfHoles { get; set; }
    public string Color { get; set; }
}
