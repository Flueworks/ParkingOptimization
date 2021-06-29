using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Threading;
using Optimizer.Optimizers;
using Optimizer.Visualizer;

namespace Optimizer
{
    class Program
    {
        [STAThread]
        static void Main2(string[] args)
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
            //b[1].Connect(a[7]);
            //c[0].Connect(a[4]);
            
            Edge.Connect(b[1], a[7], 90);
            Edge.Connect(c[0], a[4], 90);
            
            // connect first node of section d to second node of section C
            //                      B1
            // A1 A2 A3 A4 A5 A6 A7 A8 A9 A10
            //             C1
            //             C2  D1 D2 D3
            //             C3
            //d[0].Connect(c[1]);
            
            Edge.Connect(d[0], c[1], 90);
            
            
            Edge.Connect(a[5], new Source("A"), -90);
            Edge.Connect(b[4], new Source("B"), -90);
            Edge.Connect(c[2], new Source("C"), -90);
            Edge.Connect(d[2], new Source("D"), -90);
            
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
                new NotOptimized(),
            };

            VisualizerWindow window = new VisualizerWindow();
            foreach (var optimizer in optimizers)
            {
                var assignments = optimizer.AssignParkingSpots(graph.Nodes.Values.ToList(), customers);
                var score = ParkingScorer.Score(assignments);
                window.AddGraph(optimizer.GetType().Name, graph.Nodes.Values.ToList(), assignments, score);
            }

            window.ShowDialog();
        }
        
        [STAThread]
        static void Main(string[] args)
        {
            var sectionA = new ParkingGarageSection(Brunstad.SectionA);
            var sectionB1 = new ParkingGarageSection(Brunstad.SectionB1);
            var sectionB2 = new ParkingGarageSection(Brunstad.SectionB2);
            var sectionB3 = new ParkingGarageSection(Brunstad.SectionB3);
            var sectionC = new ParkingGarageSection(Brunstad.SectionC);
            var sectionD = new ParkingGarageSection(Brunstad.SectionD);
            var sectionE = new ParkingGarageSection(Brunstad.SectionE);
            var sectionF1 = new ParkingGarageSection(Brunstad.SectionF1);
            var sectionF2 = new ParkingGarageSection(Brunstad.SectionF2);
            var sectionG1 = new ParkingGarageSection(Brunstad.SectionG1);
            var sectionG2 = new ParkingGarageSection(Brunstad.SectionG2);
            var sectionU1 = new ParkingGarageSection(Brunstad.U1);
            var sectionU2 = new ParkingGarageSection(Brunstad.U2);

            var parkingGarage = new ParkingGarage();
            parkingGarage.AddSection(sectionA);
            parkingGarage.AddSection(sectionB1);
            parkingGarage.AddSection(sectionB2);
            parkingGarage.AddSection(sectionB3);
            parkingGarage.AddSection(sectionC);
            parkingGarage.AddSection(sectionD);
            parkingGarage.AddSection(sectionE);
            parkingGarage.AddSection(sectionF1);
            parkingGarage.AddSection(sectionF2);
            parkingGarage.AddSection(sectionG1);
            parkingGarage.AddSection(sectionG2);
            parkingGarage.AddSection(sectionU1);
            parkingGarage.AddSection(sectionU2);

            parkingGarage.AddHotel("X1", "G 001 HC");
            parkingGarage.AddHotel("X3", "G 035 HC");
            parkingGarage.AddHotel("X5", "G 065");
            parkingGarage.AddHotel("X7", "G 088 R");
            parkingGarage.AddHotel("X18", "G 172", 90);
            parkingGarage.AddHotel("X20", "F 001");
            parkingGarage.AddHotel("X22", "F 064");
            parkingGarage.AddHotel("X24", "F 081", -90);
            parkingGarage.AddHotel("X16", "E 049", -90);
            parkingGarage.AddHotel("X14", "D 080", 90);
            parkingGarage.AddHotel("X12", "D 035 HC");
            parkingGarage.AddHotel("X10", "C 033");
            parkingGarage.AddHotel("X4", "A 038", 90);
            parkingGarage.AddHotel("X2", "B 034 HC");

            parkingGarage.Connect("A 060", "B 002", -90);
            parkingGarage.Connect("B 025", "B 045 HC", -90);
            parkingGarage.Connect("B 044", "B 040", 90);

            parkingGarage.Connect("A 061", "D 001", 0);
            parkingGarage.Connect("D 010", "C 001", -90);
            
            parkingGarage.Connect("D 080", "E 049", 0);
            //
            parkingGarage.Connect("E 001", "G 188", -90);
            parkingGarage.Connect("E 001", "F 002", 0);
            parkingGarage.Connect("F 004", "F 066", 90);
            
            parkingGarage.Connect("G 097", "G 088 R", -90);
            
            parkingGarage.Connect("A 002", "U2 001", 90);
            parkingGarage.Connect("U2 139", "U2 140 HC", 0);
            
            parkingGarage.PropagateRouting();
            parkingGarage.AssignParkingToHotels();

            var nodes = parkingGarage.Nodes.Where(x => x.RoutingTable.Count == 0).ToList();

            var customers = CustomerGenerator.CreateCustomers(new Dictionary<string, int>()
            {
                ["X1"]  = 74, 
                ["X3"]  = 74, 
                ["X5"]  = 74, 
                ["X7"]  = 74, 
                ["X18"] = 74,
                ["X20"] = 74,
                ["X22"] = 74,
                ["X24"] = 74,
                ["X16"] = 74,
                ["X14"] = 74,
                ["X12"] = 74,
                ["X10"] = 74,
                ["X4"]  = 78, 
                ["X2"]  = 80, 
            });

            List<IParkingOptimizer> optimizers = new List<IParkingOptimizer>()
            {
                new ParkingZoneOptimizer(),
                new FirstAndBestSpotOptimizer(),
                new NotOptimized(),
            };

            VisualizerWindow window = new VisualizerWindow();
            foreach (var optimizer in optimizers)
            {
                var assignments = optimizer.AssignParkingSpots(parkingGarage.Nodes, customers);
                var score = ParkingScorer.Score(assignments);
                //var assignments = new List<ParkingAssignment>();
                //var score = new Score(0, 0, 0, 0); //ParkingScorer.Score(assignments);
                window.AddGraph(optimizer.GetType().Name, parkingGarage.Nodes, assignments, score);
            }
            
            window.ShowDialog();


        }
        
        [STAThread]
        static void Main5(string[] args)
        {
            List<Node> nodes = new List<Node>();

            var a1 = new Node("A 1");
            var a2 = new Node("A 2");
            var a3 = new Node("A 3");
            var a4 = new Node("A 4");
            
            var a22 = new Node("A 22");
            var a221 = new Node("A 221");
            var a222 = new Node("A 222");
            var a223 = new Node("A 223");
            
            
            var a23 = new Node("A 23");
            var a231 = new Node("A 231");
            var a232 = new Node("A 232");
            var a233 = new Node("A 233");
            
            
            Edge.Connect(a1, a2, 0);
            Edge.Connect(a2, a3, 0);
            Edge.Connect(a3, a4, 0);
            
            Edge.Connect(a2, a22, 90);
            Edge.Connect(a22, a221, 0);
            Edge.Connect(a221, a222, 0);
            Edge.Connect(a222, a223, 0);
            
            //Edge.Connect(a2, a23, -90);
            Edge.Connect(a23, a231, 0);
            Edge.Connect(a231, a232, 0);
            Edge.Connect(a232, a233, 0);
            
            var b1 = new Node("B 1");
            var b2 = new Node("B 2");
            var b3 = new Node("B 3");
            var b4 = new Node("B 4");
            
            var b22 =  new Node("C 220");
            var b221 = new Node("C 221");
            var b222 = new Node("C 222");
            var b223 = new Node("C 223");
            
            
            var b23 =  new Node("D 230");
            var b231 = new Node("D 231");
            var b232 = new Node("D 232");
            var b233 = new Node("D 233");
            
            
            Edge.Connect(b1, b2, 0);
            Edge.Connect(b2, b3, 0);
            Edge.Connect(b3, b4, 0);
            
            Edge.Connect(b2, b22, 90);
            Edge.Connect(b22, b221, 0);
            Edge.Connect(b221, b222, 0);
            Edge.Connect(b222, b223, 0);
            
            Edge.Connect(b2, b23, -90);
            Edge.Connect(b23, b231, 0);
            Edge.Connect(b231, b232, 0);
            Edge.Connect(b232, b233, 0);
            
            
            
            Edge.Connect(a223, b222, 90);
            
            
            nodes.Add(a1);

            List<IParkingOptimizer> optimizers = new List<IParkingOptimizer>()
            {
                //new ParkingZoneOptimizer(),
                //new FirstAndBestSpotOptimizer(),
                new NotOptimized(),
            };

            VisualizerWindow window = new VisualizerWindow();
            foreach (var optimizer in optimizers)
            {
                //var assignments = optimizer.AssignParkingSpots(parkingGarage.Nodes, customers);
                //var score = ParkingScorer.Score(assignments);
                var assignments = new List<ParkingAssignment>();
                var score = new Score(0, 0, 0, 0); //ParkingScorer.Score(assignments);
                window.AddGraph(optimizer.GetType().Name, nodes, assignments, score);
            }
            
            window.ShowDialog();


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