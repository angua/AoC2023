using Common;

namespace AoC2023Lib.Days.Day08Lib;

public class Navigator
{
    public List<Instruction> Instructions { get; private set; } = new();

    public List<Position> Positions { get; private set; } = new();

    private List<Route> _routes = new();


    private Dictionary<string, Dictionary<int, Position>> _sequences = new();
    private Dictionary<string, List<int>> _targetSteps = new();

    public int CurrentInstructionNum { get; private set; } = 0;
    public List<Position> CurrentPositions { get; private set; } = new();


    public void Parse(Filedata fileData)
    {
        // LLLLLLLRRRLRRRLRLRLRLRRLLRRRLRLLRR...
        var instructionLine = fileData.Lines[0];

        foreach (var character in instructionLine)
        {
            var inst = character switch
            {
                'L' => Instruction.Left,
                'R' => Instruction.Right,
                _ => throw new InvalidOperationException($"unknown instruction {character}")
            };
            Instructions.Add(inst);
        }

        for (int i = 2; i < fileData.Lines.Count; i++)
        {
            // DGK = (KVQ, XHR)
            Positions.Add(new Position(fileData.Lines[i]));
        }

        ConnectPositions();

        CurrentPositions = Positions.Where(p => p.Name.Last() == 'A').ToList();

        foreach (var pos in CurrentPositions)
        {
            _routes.Add(new Route()
            {
                StartPosition = pos,
            });
        }

    }

    private void ConnectPositions()
    {
        foreach (var position in Positions)
        {
            position.LeftPosition = Positions.FirstOrDefault(p => p.Name == position.LeftPositionName);
            position.RightPosition = Positions.FirstOrDefault(p => p.Name == position.RightPositionName);
        }
    }

    public long GetStepsToDestination()
    {
        var StartPosition = Positions.First(p => p.Name == "AAA");
        var EndPosition = Positions.First(p => p.Name == "ZZZ");

        var currentPosition = StartPosition;

        var currentInstructionNum = 0;
        long steps = 0;

        while (currentPosition != EndPosition)
        {
            if (Instructions[currentInstructionNum] == Instruction.Left)
            {
                currentPosition = currentPosition.LeftPosition;
            }
            else
            {
                currentPosition = currentPosition.RightPosition;
            }
            currentInstructionNum++;

            if (currentInstructionNum >= Instructions.Count)
            {
                currentInstructionNum = 0;
            }

            steps++;
        }

        return steps;
    }

    public long GetMultiStepsToDestination()
    {
        // all positions that end with A
        var currentPositions = Positions.Where(p => p.Name.Last() == 'A').ToList();

        var sequencesRun = 0;

        while (true)
        {
            // create sequence by going through instructions once
            var sequences = currentPositions.Select(p => GetSequence(p)).ToList();
            var targets = currentPositions.Select(p => GetTargetSteps(p)).ToList();

            if (targets.Any(t => t.Count > 0))
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    foreach (var target in targets[i])
                    {
                        var route = _routes[i];
                        route.TargetSteps.Add((sequencesRun, target));
                        if (route.TargetSteps.Count == 1)
                        {
                            route.FirstTargetSequence = sequencesRun;
                        }

                        if (route.TargetSteps.Count == 2)
                        {
                            route.TargetSequenceDiff = sequencesRun - route.TargetSteps[0].Item1;
                        }
                    }
                }
            }
            if (_routes.All(r => r.TargetSequenceDiff > 0))
            {
                // found all diffs, now calculate when they all meet
                // lowest common multiple

                var primeFactors = new List<List<int>>();

                for (int i = 0; i < _routes.Count; i++)
                {
                    var route = _routes[i];
                    var primes = GetPrimeFactors(route.TargetSequenceDiff);
                    primeFactors.Add(primes);
                }

                var commonPrimes = primeFactors[0];

                for (int i = 1; i < primeFactors.Count; i++)
                {
                    var intersected = commonPrimes.Intersect(primeFactors[i]);
                    var exceptCommon = commonPrimes.Except(primeFactors[i]);
                    var ecxeptNew = primeFactors[i].Except(commonPrimes);

                    commonPrimes = intersected.ToList();
                    commonPrimes.AddRange(exceptCommon);
                    commonPrimes.AddRange(ecxeptNew);
                }


                long product = commonPrimes[0];
                for (int i = 1; i < commonPrimes.Count; i++)
                {
                    product *= commonPrimes[i];
                }

                return (product - 1) * Instructions.Count + _routes.First().TargetSteps.First().Item2 + 1;








            }




            // start with the last positions of the sequences for the next instruction sequence
            currentPositions = sequences.Select(s => s.Last().Value).ToList();
            sequencesRun++;
        }




    }

    private List<int> GetPrimeFactors(int number)
    {
        var primes = new List<int>();

        for (int div = 2; div <= number; div++)
            while (number % div == 0)
            {
                primes.Add(div);
                number = number / div;
            }

        return primes;
    }

    private List<int> GetTargetSteps(Position startPos)
    {
        if (_targetSteps.TryGetValue(startPos.Name, out var targetSteps))
        {
            return targetSteps;
        }

        var sequence = GetSequence(startPos);
        targetSteps = GetTargetSteps(sequence);

        _targetSteps[startPos.Name] = targetSteps;

        return targetSteps;

    }



    private List<int> GetTargetSteps(Dictionary<int, Position> sequence)
    {
        var targetPositions = sequence.Where(p => p.Value.IsTarget);
        return targetPositions.Select(t => t.Key).ToList();
    }


    private Dictionary<int, Position> GetSequence(Position startPos)
    {
        if (_sequences.TryGetValue(startPos.Name, out var sequence))
        {
            return sequence;
        }

        sequence = new Dictionary<int, Position>();
        var currentPosition = startPos;

        for (var i = 0; i < Instructions.Count; i++)
        {
            var instruction = Instructions[i];
            if (instruction == Instruction.Left)
            {
                currentPosition = currentPosition.LeftPosition;
            }
            else
            {
                currentPosition = currentPosition.RightPosition;
            }
            sequence.Add(i, currentPosition);
        }

        _sequences[startPos.Name] = sequence;
        return sequence;
    }

    public List<Position> GetNextPositions()
    {
        var instruction = Instructions[CurrentInstructionNum];
        var newPositions = new List<Position>();

        foreach (var position in CurrentPositions)
        {
            if (instruction == Instruction.Left)
            {
                newPositions.Add(position.LeftPosition);
            }
            else
            {
                newPositions.Add(position.RightPosition);
            }
        }

        CurrentInstructionNum++;

        if (CurrentInstructionNum >= Instructions.Count)
        {
            CurrentInstructionNum = 0;
        }

        CurrentPositions = newPositions;
        return newPositions;
    }
}
