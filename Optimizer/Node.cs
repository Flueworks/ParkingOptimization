using System.Collections.Generic;

namespace Optimizer
{
    public class Node
    {
        public string Id { get; }

        public Node(string id)
        {
            Id = id;
        }

        public List<Node> Edges { get; } = new List<Node>();

        public void Connect(Node node)
        {
            node.Edges.Add(this);
            Edges.Add(node);
        }

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
        
        public string WriteEdges()
        {
            string edges = $"{Id}: ";
            foreach (var node in Edges)
            {
                edges += $"{node.Id}, ";
            }
            return edges;
        }
    }
}