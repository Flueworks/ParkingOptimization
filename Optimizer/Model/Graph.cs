using System.Collections.Generic;

namespace Optimizer
{
    /// <summary>
    /// Represents the parking garage
    /// </summary>
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
                
                Edge.Connect(previousNode, node, 0);
                previousNode = node;
            }

            return result;
        }
    }
}