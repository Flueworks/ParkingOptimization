using System;
using System.Collections.Generic;

namespace Optimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var graph = new Graph();
            var a = graph.GenerateNodes("A", 10);
            var b = graph.GenerateNodes("B", 5);
            var c = graph.GenerateNodes("C", 3);
            
            c[0].Connect(a[4]);
            b[1].Connect(a[7]);
            
            a[5].PropagateRoutingTable("A", 0);
            b[3].PropagateRoutingTable("B", 0);
            
            foreach (var node in graph.Nodes)
            {
                Console.WriteLine($"{node.Value.WriteRoutingTable()}" );
            }

            Console.WriteLine("Hello World!");
        }

    }

    public class Graph
    {
        public Dictionary<string, Node> Nodes { get; } = new Dictionary<string, Node>();


        public List<Node> GenerateNodes(string prefix, int count)
        {
            int id = 1;
            List<Node> result = new List<Node>();
            Node previousNode = null;
            for (int i = 0; i < count; i++)
            {
                var nodeId = prefix + "-" + id++;
                var node = new Node(nodeId);
                Nodes.Add(nodeId, node);
                result.Add(node);
                
                previousNode?.Connect(node);
                previousNode = node;
            }

            return result;
        }
    }

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
