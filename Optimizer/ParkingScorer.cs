using System;
using System.Collections.Generic;
using System.Linq;
using Optimizer.Optimizers;

namespace Optimizer
{
    internal class ParkingScorer
    {
        public static Score Score(List<ParkingAssignment> assignments)
        {
            var max = assignments.Max(x => x.GetWalkingDistance());
            var min = assignments.Min(x => x.GetWalkingDistance());
            var average = assignments.Average(x => x.GetWalkingDistance());
            var median = assignments[assignments.Count / 2].GetWalkingDistance();
            
            Console.WriteLine($"Min: {min}, Max: {max}, Avg: {average}, Median: {median}");
            return new Score(min, max, average, median);
        }
    }

    public class Score
    {
        public int Min { get; }
        public int Max { get; }
        public double Average { get; }
        public int Median { get; }

        public Score(int min, int max, double average, int median)
        {
            Min = min;
            Max = max;
            Average = average;
            Median = median;
        }
    }
}