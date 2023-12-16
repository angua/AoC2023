namespace AoC2023Lib.Days.Day15Lib;

public class Box
{
    public int Number {  get; set; }

    public List<Lens> Lenses { get; set; } = new();

    internal int GetFocusingPower()
    {
        var focusingPower = 0;
        for (int i = 0; i < Lenses.Count; i++)
        {
            focusingPower += (Number + 1) * (i + 1) * Lenses[i].FocalLength;
        }
        return focusingPower;
    }
}
