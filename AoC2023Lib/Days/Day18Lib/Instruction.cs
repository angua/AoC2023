using System.Numerics;

namespace AoC2023Lib.Days.Day18Lib;

public class Instruction
{
    private string _line;

    public Instruction(string line)
    {
        _line = line;

        // R 6 (#70c710)
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Dir = GetDirection(parts[0]);
        Distance = int.Parse(parts[1]);
        var colorString = parts[2];
        colorString = colorString.Replace("(", string.Empty);
        Color = colorString.Replace(")", string.Empty);
    }

    public Direction Dir { get; set; }
    public int Distance { get; set; }

    public string Color { get; set; }

    public Direction DirFromColor { get; set; }
    public int DistanceFromColor { get; set; }


    internal void ConvertColorToRealInstruction()
    {
        // last digit is direction
        var directionNum = int.Parse(Color.Last().ToString());
        DirFromColor = GetDirection(directionNum);

        // remove last digit
        var distancePart = Color.Remove(Color.Length - 1);
        distancePart = distancePart.Replace("#", string.Empty);

        // convert hex number
        DistanceFromColor = int.Parse(distancePart, System.Globalization.NumberStyles.HexNumber);
    }

    //  0 means R, 1 means D, 2 means L, and 3 means U.
    private Direction GetDirection(string input)
    {
        return input switch
        {
            "R" => Direction.Right,
            "D" => Direction.Down,
            "L" => Direction.Left,
            "U" => Direction.Up,
            _ => throw new InvalidOperationException($"Invalid direction {input}")
        };
    }

    //  0 means R, 1 means D, 2 means L, and 3 means U.
    private Direction GetDirection(int input)
    {
        return input switch
        {
            0 => Direction.Right,
            1 => Direction.Down,
            2 => Direction.Left,
            3 => Direction.Up,
            _ => throw new InvalidOperationException($"Invalid direction {input}")
        };
    }
}
