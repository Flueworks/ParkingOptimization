using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    internal class ParkingScorer
    {
        public static void Score(List<ParkingAssignment> assignments)
        {
            var max = assignments.Max(x => x.GetWalkingDistance());
            var min = assignments.Min(x => x.GetWalkingDistance());
            var average = assignments.Average(x => x.GetWalkingDistance());
            var median = assignments[assignments.Count / 2].GetWalkingDistance();
            
            Console.WriteLine($"Min: {min}, Max: {max}, Avg: {average}, Median: {median}");
        }
    }
}