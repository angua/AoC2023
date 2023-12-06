using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day06Lib
{
    public class BoatRacer
    {
        // <time, distance>
        public Dictionary<int, int> Entries { get; set; } = new();

        public void Parse(Filedata fileData)
        {
        // Time:        35     69     68     87
        // Distance: 213   1168   1086   1248

            var timeParts = fileData.Lines[0].Split(':', StringSplitOptions.RemoveEmptyEntries);
            var timeStrings = timeParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            var distanceParts = fileData.Lines[1].Split(':', StringSplitOptions.RemoveEmptyEntries);
            var distanceStrings = distanceParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < timeStrings.Count; i++)
            {
                Entries.Add(int.Parse(timeStrings[i]), int.Parse(distanceStrings[i]));
            }
        }

        public int GetWinningRangeProduct()
        {
            var ranges = new List<int>();

            foreach (var entry in Entries) 
            {
                ranges.Add(GetWinningRangeCount(entry.Key, entry.Value));
            }

            var product = ranges[0];

            for (int i = 1; i < ranges.Count; i++)
            {
                product *= ranges[i];
            }

            return product;

        }

        private int GetWinningRangeCount(long time, long distance)
        {
            // totalTime = buttonTime + moveTime
            // distance = buttonTime * moveTime
            // distance = buttonTime * totalTime - buttonTime^2
            // parabola with maximum at totaltime / 2
            // buttonTime^2 - buttonTime * totalTime + distance = 0
            // buttonTime = totalTime / 2 +/- sqrt(totalTime^2 / 4 - distance)

            var firstEquationPart = (double)time / 2;
            var secondEquationPart = Math.Sqrt(Math.Pow((double)time, 2) / 4 - distance);

            var firstWinningPoint = Math.Ceiling( firstEquationPart - secondEquationPart);
            var lastWinningPoint = Math.Floor(firstEquationPart + secondEquationPart);

            return (int)(lastWinningPoint - firstWinningPoint + 1);

        }

        public long GetBigWinningRange()
        {
            // parse entries together
            var timeString = string.Join("", Entries.Keys);
            var distanceString = string.Join("", Entries.Values);

            long time = long.Parse(timeString);
            long distance = long.Parse(distanceString);

            return GetWinningRangeCount(time, distance);

        }
    }
}
