using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace AoC2023Lib.Days.Day05Lib
{
    public class Gardening
    {
        private Filedata _fileData;

        private List<Mapping> _mappings = new();

        public List<long> Seeds { get; private set; }

        private List<(long, long)> _seedRanges = new();


        public void Parse(Filedata fileData)
        {
            _fileData = fileData;

            // seeds: 79 14 55 13
            var seedLine = fileData.Lines[0];

            var seedParts = seedLine.Split(':', StringSplitOptions.RemoveEmptyEntries);
            var seedNumStrings = seedParts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            Seeds = seedNumStrings.Select(s => long.Parse(s)).ToList();

            var currentLineNum = 1;

            var currentDict = new Dictionary<long, long>();

            while (currentLineNum < fileData.Lines.Count)
            {
                var currentLine = fileData.Lines[currentLineNum];

                if (currentLine.Contains("seed-to-soil"))
                {
                    CreateMappings(Resource.Seed, Resource.Soil, ref currentLineNum);
                }
                else if (currentLine.Contains("soil-to-fertilizer"))
                {
                    CreateMappings(Resource.Soil, Resource.Fertilizer, ref currentLineNum);
                }
                else if (currentLine.Contains("fertilizer-to-water"))
                {
                    CreateMappings(Resource.Fertilizer, Resource.Water, ref currentLineNum);
                }
                else if (currentLine.Contains("water-to-light"))
                {
                    CreateMappings(Resource.Water, Resource.Light, ref currentLineNum);
                }
                else if (currentLine.Contains("light-to-temperature"))
                {
                    CreateMappings(Resource.Light, Resource.Temperature, ref currentLineNum);
                }
                else if (currentLine.Contains("temperature-to-humidity"))
                {
                    CreateMappings(Resource.Temperature, Resource.Humidity, ref currentLineNum);
                }
                else if (currentLine.Contains("humidity-to-location"))
                {
                    CreateMappings(Resource.Humidity, Resource.Location, ref currentLineNum);
                }

                currentLineNum++;
            }
        }

        private void CreateMappings(Resource sourceResource, Resource destinationResource, ref int currentLineNum)
        {
            currentLineNum++;
            var currentLine = _fileData.Lines[currentLineNum];
            while (!string.IsNullOrEmpty(currentLine))
            {
                var parts = currentLine.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var destinationstart = long.Parse(parts[0]);
                var sourceStart = long.Parse(parts[1]);
                var range = long.Parse(parts[2]);

                var mapping = new Mapping()
                {
                    SourceResource = sourceResource,
                    DestinationResource = destinationResource,
                    SourceStart = sourceStart,
                    DestinationStart = destinationstart,
                    Range = range
                };

                _mappings.Add(mapping);

                currentLineNum++;

                currentLine = (currentLineNum < _fileData.Lines.Count) ? _fileData.Lines[currentLineNum] : null;
            }
        }


        public Dictionary<long, long> GetSeedLocations()
        {
            var result = new Dictionary<long, long>();
            foreach (var seedNumber in Seeds)
            {
                var locationNumber = GetLocationForSeed(seedNumber);
                result.Add(seedNumber, locationNumber);
            }
            return result;
        }


        private long GetLocationForSeed(long seedNumber)
        {
            var sourceResource = new ResourceNumber()
            {
                ResourceType = Resource.Seed,
                Number = seedNumber
            };

            while (sourceResource.ResourceType != Resource.Location)
            {
                sourceResource = GetDestination(sourceResource);
            }

            return sourceResource.Number;
        }

        private ResourceNumber GetDestination(ResourceNumber sourceResource)
        {
            var resourceMappings = _mappings.Where(m => m.SourceResource == sourceResource.ResourceType).ToList();

            foreach (var mapping in resourceMappings)
            {
                if (TryFindSourceMapping(sourceResource, mapping, out var destination))
                {
                    // found in mapping
                    return destination;
                }
            }

            // not found in mapping, direct mapping
            return new ResourceNumber()
            {
                ResourceType = GetNextResource(sourceResource.ResourceType),
                Number = sourceResource.Number
            };
        }

        private Resource GetNextResource(Resource resourceType)
        {
            return (Resource)((int)resourceType + 1);
        }
        private Resource GetPreviousResource(Resource resourceType)
        {
            return (Resource)((int)resourceType - 1);
        }

        private bool TryFindSourceMapping(ResourceNumber sourceResource, Mapping mapping, out ResourceNumber destination)
        {
            destination = new ResourceNumber();

            var num = sourceResource.Number;

            if (num < mapping.SourceStart)
            {
                return false;
            }

            var offset = num - mapping.SourceStart;

            if (offset >= mapping.Range)
            {
                return false;
            }

            destination.Number = mapping.DestinationStart + offset;
            destination.ResourceType = mapping.DestinationResource;

            return true;
        }

        public long GetClosestLocation(Dictionary<long, long> seedLocations)
        {
            return seedLocations.Min(l => l.Value);
        }

        public long GetClosestRangeLocation(Dictionary<long, long> seedRangeLocations)
        {
            // parse seed ranges
            for (var i = 0; i < Seeds.Count; i++)
            {
                var pair = (Seeds[i], Seeds[++i]);
                _seedRanges.Add(pair);
            }

            // count up until we find a location with matching seed 
            var location = 0;
            while (true)
            {
                // reverse engineer to find seed for location
                var seed = GetSeedForLocation(location);
                if (InSeedRange(seed))
                {
                    return location;
                }
                location++;
            }


        }

        private long GetSeedForLocation(long location)
        {
            var destinationResource = new ResourceNumber()
            {
                ResourceType = Resource.Location,
                Number = location
            };

            while (destinationResource.ResourceType != Resource.Seed)
            {
                destinationResource = GetSource(destinationResource);
            }

            return destinationResource.Number;
        }

        private ResourceNumber GetSource(ResourceNumber destinationResource)
        {
            var resourceMappings = _mappings.Where(m => m.DestinationResource == destinationResource.ResourceType).ToList();

            foreach (var mapping in resourceMappings)
            {
                if (TryFindDestinationMapping(destinationResource, mapping, out var source))
                {
                    // found in mapping
                    return source;
                }
            }

            // not found in mapping, direct mapping
            return new ResourceNumber()
            {
                ResourceType = GetPreviousResource(destinationResource.ResourceType),
                Number = destinationResource.Number
            };
        }

        

        private bool TryFindDestinationMapping(ResourceNumber destinationResource, Mapping mapping, out ResourceNumber source)
        {
            source = new ResourceNumber();

            var num = destinationResource.Number;

            if (num < mapping.DestinationStart)
            {
                return false;
            }

            var offset = num - mapping.DestinationStart;

            if (offset >= mapping.Range)
            {
                return false;
            }

            source.Number = mapping.SourceStart + offset;
            source.ResourceType = mapping.SourceResource;

            return true;
            throw new NotImplementedException();
        }

        private bool InSeedRange(long seed)
        {
            // (start, range)
            foreach (var pair in _seedRanges)
            {
                if (seed >= pair.Item1 && seed - pair.Item1 < pair.Item2)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
