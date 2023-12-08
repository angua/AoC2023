using Common;

namespace AoC2023Lib.Days.Day08Lib;

public class Navigator
{
    public List<Instruction> Instructions { get; private set; } = new();

    public List<Position> Positions { get; private set; } = new();


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
                var commonTargets = targets[0];

                for (int i = 1; i < targets.Count; i++)
                {
                    if (commonTargets.Count > 0)
                    {
                        var newTargets = commonTargets.Intersect(targets[i]).ToList();
                        commonTargets = newTargets;
                    }
                }
                if (commonTargets.Count > 0)
                {
                    return commonTargets.First() + sequencesRun * Instructions.Count;
                }

            }



            // start with the last positions of the sequences for the next instruction sequence
            currentPositions = sequences.Select(s => s.Last().Value).ToList();
            sequencesRun++;
        }




        long steps = 0;
        var currentInstructionNum = 0;

        while (true)
        {
            var newPositions = new List<Position>();

            var instruction = Instructions[currentInstructionNum];

            foreach (var position in currentPositions)
            {
                if (Instructions[currentInstructionNum] == Instruction.Left)
                {
                    newPositions.Add(position.LeftPosition);
                }
                else
                {
                    newPositions.Add(position.RightPosition);
                }
            }

            currentInstructionNum++;
            if (currentInstructionNum >= Instructions.Count)
            {
                currentInstructionNum = 0;
            }
            steps++;

            // all positions end with Z
            if (newPositions.All(p => p.IsTarget))
            {
                break;
            }

            currentPositions = newPositions;
        }

        return steps;
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
