using Common;

namespace AoC2023Lib.Days.Day01Lib;

public class Calibrator
{
    public List<string> CalibrationInput { get; private set; } = new();
    private List<string> __spelledCalibrationNumbers = new();

    private Dictionary<string, char> _spelledNums = new()
    {
        {"one", '1' },
        {"two", '2' },
        {"three", '3' },
        {"four", '4' },
        {"five", '5' },
        {"six", '6' },
        {"seven", '7' },
        {"eight", '8' },
        {"nine", '9' },
    };

    public void Parse(Filedata fileData)
    {
        foreach (var line in fileData.Lines)
        {
            CalibrationInput.Add(line);
        }
    }

    public List<int> GetCalibrationNumbers()
    {
        return GetCalibrationNumbers(CalibrationInput);
    }


    public List<int> GetCalibrationNumbers(List<string> inputStrings)
    {
        var result = new List<int>();
        foreach (var input in inputStrings)
        {
            var numInput = input.Where(c => Char.IsDigit(c)).ToList();
            var firstDigit = numInput.First();
            var lastDigit = numInput.Last();
            var numStr = string.Join("", firstDigit, lastDigit);
            result.Add(int.Parse(numStr));
        }
        return result;
    }

    public List<string> GetSpelledCalibrationNums()
    {
        var result = new List<string>();

        foreach (var input in CalibrationInput)
        {
            // find strings to replace in input (can overlap)
            var replacements = new List<StringReplaceNum>();
            foreach (var replacement in _spelledNums)
            {
                var replaceStr = replacement.Key;
                var startPos = 0;
                var offset = 0;

                var subStr = input.Substring(startPos, input.Length - startPos);

                while (subStr.Contains(replaceStr))
                {
                    var pos = subStr.IndexOf(replaceStr);
                    replacements.Add(new StringReplaceNum()
                    {
                        StartPos = offset + pos,
                        RemoveCharCount = replaceStr.Length,
                        Replacement = replacement.Value
                    });

                    startPos = pos + 1;
                    offset += startPos;
                    subStr = subStr.Substring(startPos, subStr.Length - startPos);
                }
            }

            var charList = input.Select(c => new CharReplace()
            {
                Character = c
            }).ToList();

            // mark for deletion
            foreach (var replacement in replacements)
            {
                for (int i = 0; i < replacement.RemoveCharCount; i++)
                {
                    charList[replacement.StartPos + i].Remove = true;
                }
            }

            // replace first character with number and mark for non-deletion
            foreach (var replacement in replacements)
            {
                charList[replacement.StartPos].Character = replacement.Replacement;
                charList[replacement.StartPos].Remove = false;
            }

            var newchar = charList.Where(c => c.Remove == false).Select(c => c.Character);

            result.Add(string.Join("", newchar));
        }
        return result;
    }

    public int GetCalibrationSum(List<int> calibrationNumbers)
    {
        return calibrationNumbers.Sum();
    }


}
