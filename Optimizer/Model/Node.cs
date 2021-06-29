using System;
using System.Collections.Generic;

namespace Optimizer
{
    /// <summary>
    /// Represents a parking spot
    /// </summary>
    public class Node
    {
        public string Id { get; }
        public string BranchId => Id.Split(" ")[0];

        public Node(string id)
        {
            Id = id;
        }

        public List<Edge> Edges { get; } = new List<Edge>();

        public Dictionary<string, int> RoutingTable { get; } = new Dictionary<string, int>();

        public void PropagateRoutingTable(string name, int distance)
        {
            var value = distance + 1;
            
            if (!RoutingTable.ContainsKey(name))
                RoutingTable.Add(name, value);
            else if (RoutingTable[name] > value)
                RoutingTable[name] = value;
            else
                return; // we already have a shorter route to destination

            foreach (var edge in Edges)
            {
                // propagate routing information
                edge.PropagateRoutingTable(name, value);
            }
        }

        public string WriteRoutingTable()
        {
            string routingTable = $"{Id}: ";
            foreach (var val in RoutingTable)
            {
                routingTable += $"{val.Key}: {val.Value}, ";
            }
            return routingTable;
        }
    }

    public class Edge
    {
        public Node Node { get; }
        public int Angle { get; }

        public static void Connect(Node a, Node b, int angle)
        {
            if(a == null || b == null)
                return;
            var edge1 = new Edge(b, angle);
            var edge2 = new Edge(a, 0); //angle does not work...
            
            a.Edges.Add(edge1);
            b.Edges.Add(edge2);
        }

        private Edge(Node node, int angle)
        {
            Node = node;
            Angle = angle;
        }

        public void PropagateRoutingTable(string name, int value) => Node.PropagateRoutingTable(name, value);
    }

    public class Source : Node
    {
        /// <inheritdoc />
        public Source(string id) : base(id)
        {
        }
    }
}