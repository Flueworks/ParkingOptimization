using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizer.Optimizers
{
    /// <summary>
    /// Finds the first and best spot for the customer. Does not adhere to zones.
    /// </summary>
    public class FirstAndBestSpotOptimizer : IParkingOptimizer
    {
        public List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.ToList();

            List<ParkingAssignment> result = new List<ParkingAssignment>();
            foreach (var customer in customers)
            {
                var bestParkingSpot = freeParkingSpots.OrderBy(x => x.RoutingTable[customer.Hotel]).First();
                freeParkingSpots.Remove(bestParkingSpot);
                result.Add(new ParkingAssignment(customer,bestParkingSpot));
            }

            return result;
        }
    }

    public class NotOptimized : IParkingOptimizer
    {
        /// <inheritdoc />
        public List<ParkingAssignment> AssignParkingSpots(List<Node> spots, List<Customer> customers)
        {
            List<ParkingAssignment> result = new List<ParkingAssignment>();

            for (int i = 0; i < Math.Min(customers.Count, spots.Count); i++)
            {
                result.Add(new ParkingAssignment(customers[i], spots[i]));
            }

            return result;
        }
    }

    public interface IParkingOptimizer
    {
        List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers);
    }
}