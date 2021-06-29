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

    class PrioritizedParkingZoneOptimizer : IParkingOptimizer
    {
        /// <inheritdoc />
        public List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.ToList();
            var result = new List<ParkingAssignment>();
            var remainingCustomers = customers.ToList();

            var sections = graph.GroupBy(x => x.BranchId)
                .ToDictionary(x=>x.Key, x=> new List<Node>(x.Select(y=>y).OrderBy(n => n.RoutingTable[x.Key]).ToList()));
            foreach (var customer in customers)
            {
                var section = sections[customer.Hotel];

                if (section.Count > 0)
                {
                    var spot = section.First();
                    section.Remove(spot);
                    freeParkingSpots.Remove(spot);
                    result.Add(new ParkingAssignment(customer, spot));
                    remainingCustomers.Remove(customer);
                }
                else
                {
                    // if customer has priority, try to find a free parking spot close by.
                    var bestParkingSpot = freeParkingSpots.OrderBy(x => x.RoutingTable[customer.Hotel]).First();
                    if(bestParkingSpot.RoutingTable[customer.Hotel] > 20)
                        continue;
                    freeParkingSpots.Remove(bestParkingSpot);
                    sections[bestParkingSpot.BranchId].Remove(bestParkingSpot);
                    result.Add(new ParkingAssignment(customer,bestParkingSpot));
                    remainingCustomers.Remove(customer);
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
    
    class FillParkingZoneOptimizer : IParkingOptimizer
    {
        /// <inheritdoc />
        public List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.ToList();
            var result = new List<ParkingAssignment>();

            foreach (var hotel in customers.OrderBy(x=>x.Hotel).GroupBy(x=>x.Hotel))
            {
                var hotelParkingSpots = new Queue<Node>(freeParkingSpots.OrderBy(x=>x.RoutingTable[hotel.Key]).ToList());
                foreach (var customer in hotel)
                {
                    var spot = hotelParkingSpots.Dequeue();
                    freeParkingSpots.Remove(spot);
                    result.Add(new ParkingAssignment(customer, spot));
                }
            }

            return result;
        }
    }
    
    class NiceFillParkingZoneOptimizer : IParkingOptimizer
    {
        /// <inheritdoc />
        public List<ParkingAssignment> AssignParkingSpots(List<Node> graph, List<Customer> customers)
        {
            var freeParkingSpots = graph.ToList();
            var result = new List<ParkingAssignment>();

            int slots = 3;
            for (int i = 0; i < customers.Count; i+= customers.Count/slots)
            {
                foreach (var hotel in customers.Skip(i).Take(customers.Count/slots).OrderBy(x=>x.Hotel).GroupBy(x=>x.Hotel))
                {
                    var hotelParkingSpots = new Queue<Node>(freeParkingSpots.OrderBy(x=>x.RoutingTable[hotel.Key]).ToList());
                    foreach (var customer in hotel)
                    {
                        var spot = hotelParkingSpots.Dequeue();
                        freeParkingSpots.Remove(spot);
                        result.Add(new ParkingAssignment(customer, spot));
                    }
                }
            }

            return result;
        }
    }
}