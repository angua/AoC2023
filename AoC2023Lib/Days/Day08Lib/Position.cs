﻿namespace AoC2023Lib.Days.Day08Lib;

public class Position
{
    public Position(string input)
    {
        // DGK = (KVQ, XHR)
        var parts = input.Split('=', StringSplitOptions.RemoveEmptyEntries);

        Name = parts[0].Trim();

        var destinationParts = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
        LeftPositionName = destinationParts[0].Replace("(", "").Trim();
        RightPositionName = destinationParts[1].Replace(")", "").Trim();

        LastChar = Name.Last();

        if (LastChar == 'Z')
        {
            IsTarget = true;
        }
    }

    public char LastChar { get; set; }

    public string Name { get; set; }
    public string LeftPositionName { get; set; }
    public string RightPositionName { get; set; }

    public Position LeftPosition { get; set; }
    public Position RightPosition { get; set; }

    public bool IsTarget { get; set; } = false;
}
