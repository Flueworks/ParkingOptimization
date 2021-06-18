using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    public class ParkingZoneOptimizer : IParkingOptimizer
    {
        public List<ParkingAssignment> AssignParkingSpots(Graph graph, List<Customer> customers)
        {
            var sections = graph.Nodes.Values.GroupBy(x => x.Id.Split("-")[0]).ToDictionary(x=>x.Key, x=>x.Select(y=>y).ToList());

            List<ParkingAssignment> result = new List<ParkingAssignment>();

            List<Node> freeParkingSpots = graph.Nodes.Values.ToList();
            List<Customer> remainingCustomers = customers.ToList();
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