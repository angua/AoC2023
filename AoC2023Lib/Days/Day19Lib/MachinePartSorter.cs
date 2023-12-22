using Common;

namespace AoC2023Lib.Days.Day19Lib;

public class MachinePartSorter
{
    public Dictionary<string, Workflow> Workflows { get; set; } = new();

    public List<PartRating> PartRatings { get; set; } = new();


    private List<PartRating> _accepted = new();
    private List<PartRating> _rejected = new();



    public void Parse(Filedata fileData)
    {
        var i = 0;
        // first part contains workflows
        // x{a<2006:qkq,m>2090:A,rfg}
        for (; i < fileData.Lines.Count; i++)
        {
            var line = fileData.Lines[i];

            if (string.IsNullOrEmpty(line))
            {
                i++;
                break;
            }

            var workflow = new Workflow(line);

            Workflows.Add(workflow.Name, workflow);
        }

        for (; i < fileData.Lines.Count; i++)
        {
            // part ratings
            // { x = 787,m = 2655,a = 1222,s = 2876}
            var line = fileData.Lines[i];
            line = line.Replace("{", "");
            line = line.Replace("}", "");

            PartRatings.Add(new PartRating(line));
        }
    }

    public int GetRatingSum()
    {
        Sort();

        return _accepted.Select(i => i.RatingSum).Sum();
    }




    public void Sort()
    {
        var queue = new Dictionary<string, List<PartRating>>();
        var nextQueue = new Dictionary<string, List<PartRating>>();

        // start by adding all parts to workflow "in"
        foreach (var item in PartRatings)
        {
            AddToWorkFlow(queue, "in", item);
        }

        // go through each Workflow to process items
        while (true)
        {
            if (!queue.Any(w => w.Value.Count > 0))
            {
                break;
            }

            foreach (var workflowQueue in queue)
            {
                var name = workflowQueue.Key;
                var items = workflowQueue.Value.ToList();
                ProcessItems(name, items, nextQueue);
            }

            queue = nextQueue;
            nextQueue = new Dictionary<string, List<PartRating>>();
        }
    }

    private void ProcessItems(string name, List<PartRating> items, Dictionary<string, List<PartRating>> nextQueue)
    {
        var workflow = Workflows[name];

        foreach (var item in items)
        {
            var destination = workflow.Process(item);

            if (destination == "A")
            {
                _accepted.Add(item);
            }
            else if (destination == "R")
            {
                _rejected.Add(item);
            }
            else
            {
                AddToWorkFlow(nextQueue, destination, item);
            }
        }
    }

    private void AddToWorkFlow(Dictionary<string, List<PartRating>> queue, string workflow, PartRating item)
    {
        if (!queue.TryGetValue(workflow, out var partsList))
        {
            partsList = new List<PartRating>();
            queue.Add(workflow, partsList);
        }
        partsList.Add(item);
    }

    public static Category GetCategory(string input)
    {
        return input switch
        {
            "x" => Category.X,
            "m" => Category.M,
            "a" => Category.A,
            "s" => Category.S
        };
    }

}
