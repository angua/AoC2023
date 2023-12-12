using System.Collections.Generic;
using System.Linq;

namespace Common;

public class MathUtils
{
    public static List<List<int>> GetAllCombinations(int allElements , int selectedElements)
    {
        var allCombinations = new List<List<int>>();
        GetCombinations(allElements, selectedElements, new List<int>(), allCombinations);
        return allCombinations;
    }

    private static void GetCombinations(int positions, int remainingelements, List<int> previousplacements, List<List<int>> allCombinations)
    {
        if (remainingelements == 0)
        {
            allCombinations.Add(previousplacements);
            return;
        }
        var start = 0;
        if (previousplacements.Any())
        {
            start = previousplacements.Last() + 1;
        }

        var end = positions - remainingelements + 1;

        for (int i = start; i < end; i++)
        {
            var newplacements = new List<int>(previousplacements);
            newplacements.Add(i);
            GetCombinations(positions, remainingelements - 1, newplacements, allCombinations);
        }
    }


}
