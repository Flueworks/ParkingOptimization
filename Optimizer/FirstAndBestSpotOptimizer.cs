using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    /// <summary>
    /// Finds the first and best spot for the customer. Does not adhere to zones.
    /// </summary>
    public class FirstAndBestSpotOptimizer : IParkingOptimizer
    {
        public List<ParkingAssignment> AssignParkingSpots(Graph graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.Nodes.Values.ToList();

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

    public interface IParkingOptimizer
    {
        List<ParkingAssignment> AssignParkingSpots(Graph graph, List<Customer> customers);
    }
}