using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph();
            // Create four sections
            var a = graph.GenerateNodes("A", 10);
            var b = graph.GenerateNodes("B", 5);
            var c = graph.GenerateNodes("C", 3);
            var d = graph.GenerateNodes("D", 3);
            

            
            // connect second node of section B with 8th node of section A
            // connect first node of section C with 5th node of section A
            //                      B3,B4,B5
            //                      B2
            //                      B1
            // A1 A2 A3 A4 A5 A6 A7 A8 A9 A10
            //             C1
            //             C2
            //             C3
            b[1].Connect(a[7]);
            c[0].Connect(a[4]);
            
            // connect first node of section d to second node of section C
            //                      B1
            // A1 A2 A3 A4 A5 A6 A7 A8 A9 A10
            //             C1
            //             C2  D1 D2 D3
            //             C3
            d[0].Connect(c[1]);
            
            // Set entrance to hotel A at sixth node in section A 
            a[5].PropagateRoutingTable("A", 0);
            
            // Set entrance to hotel B at fifth node in section B
            b[4].PropagateRoutingTable("B", 0);

            c[2].PropagateRoutingTable("C", 0);
            d[2].PropagateRoutingTable("D", 0);

            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"{node.Value.WriteRoutingTable()}" );
            }

            var customers = CustomerGenerator.CreateCustomers(new Dictionary<string, int>()
            {
                ["A"] = a.Count,
                ["B"] = b.Count,
                ["C"] = c.Count,
                ["D"] = d.Count
            });

            List<IParkingOptimizer> optimizers = new List<IParkingOptimizer>()
            {
                new ParkingZoneOptimizer(),
                new FirstAndBestSpotOptimizer(),
            };

            foreach (var optimizer in optimizers)
            {
                Console.WriteLine(optimizer.GetType().Name);
                var assignments = optimizer.AssignParkingSpots(graph, customers);
                ParkingScorer.Score(assignments);
                Console.WriteLine();
            }
        }
    }

    public static class CustomerGenerator
    {
        public static List<Customer> CreateCustomers(Dictionary<string, int> customersPerHotel)
        {
            List<Customer> customers = new List<Customer>();
            foreach (var val in customersPerHotel)
            {
                for (int i = 0; i < val.Value; i++)
                {
                    Customer customer = new Customer()
                    {
                        Hotel = val.Key
                    };
                    customers.Add(customer);
                }
            }

            // static seed the same so order of customers are always the same,
            // so we can compare results between runs.
            // Alter this number to get different permutation
            Random r = new Random(1234); 
            customers = customers.OrderBy(_ => r.Next()).ToList();
            for (var i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                customer.Priority = i;
            }

            return customers;
        }
    }
}