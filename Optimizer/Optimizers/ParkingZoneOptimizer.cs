using System.Collections.Generic;
using System.Linq;

namespace Optimizer.Optimizers
{
    public class ParkingZoneOptimizer : IParkingOptimizer
    {
        // currently broken because hotels are named X1 instead of A, B, C etc...
        public List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers)
        {
            var sections = graph.GroupBy(x => x.BranchId).ToDictionary(x=>x.Key, x=>x.Select(y=>y).ToList());

            var result = new List<ParkingAssignment>();

            var freeParkingSpots = graph.ToList();
            var remainingCustomers = customers.ToList();
            foreach (var section in sections)
            {
                var parkingSpotsInSection = section.Value;
                var customerForHotel = customers.Where(x => x.Hotel == section.Key).ToList();

                while (parkingSpotsInSection.Any() && customerForHotel.Any())
                {
                    result.Add(new ParkingAssignment(customerForHotel[0], parkingSpotsInSection[0]));
                    freeParkingSpots.Remove(parkingSpotsInSection[0]);
                    remainingCustomers.Remove(customerForHotel[0]);
                    parkingSpotsInSection.RemoveAt(0);
                    customerForHotel.RemoveAt(0);
                }
            }

            foreach (var customer in remainingCustomers)
            {
                var bestParkingSpot = freeParkingSpots.OrderBy(x => x.RoutingTable[customer.Hotel]).First();
                freeParkingSpots.Remove(bestParkingSpot);
                result.Add(new ParkingAssignment(customer,bestParkingSpot));
            }

            return result;
        }
    }
}