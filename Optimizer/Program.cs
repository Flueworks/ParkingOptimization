using System;

namespace Optimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph();
            // Create three sections
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
            
            // Set entrance to hotel B at fourth node in section B
            b[3].PropagateRoutingTable("B", 0);
            
            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"{node.Value.WriteRoutingTable()}" );
            }

            Console.WriteLine("Hello World!");
        }
    }
}
