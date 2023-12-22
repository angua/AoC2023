using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023Lib.Days.Day19Lib
{
    public class Condition
    {

        public Condition(string input)
        {
            // a<2006:qkq
            ConditionCategory = MachinePartSorter.GetCategory(input[0].ToString());
            Operator = input[1].ToString();

            Num = int.Parse(input.Substring(2));
        }

        public Category ConditionCategory { get; set; }
        public int Num {  get; set; }
        public string Operator { get; set; }

        public bool CheckCondition(PartRating rating)
        {
            return Operator switch
            {
                "<" => rating.Ratings[ConditionCategory] < Num,
                ">" => rating.Ratings[ConditionCategory] > Num,
                _ => throw new InvalidOperationException($"Unknown operator {Operator}")
            };
        }
    }
}
