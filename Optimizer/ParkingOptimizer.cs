using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    public class ParkingOptimizer
    {
        public static void AssignParkingSpots(Graph graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.Nodes.Values.ToList();

            foreach (var customer in customers)
            {
                var bestParkingSpot = freeParkingSpots.OrderBy(x => x.RoutingTable[customer.Hotel]).First();
                customer.ParkingSpot = bestParkingSpot;
                freeParkingSpots.Remove(bestParkingSpot);
            }
            
            
        }
    }
}