using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prog7312POEST10071737.Services
{
    internal class Graph
    {
        private Dictionary<string, GraphNode> nodes;
        private List<(string Source, string Target)> edges;

        public Graph()
        {
            nodes = new Dictionary<string, GraphNode>();
            edges = new List<(string Source, string Target)>();
        }

        public void AddNode(string id, string label, NodeType type, object data)
        {
            if (!nodes.ContainsKey(id))
            {
                nodes[id] = new GraphNode(id, label, type, data);
            }
        }

        public void AddEdge(string sourceId, string targetId)
        {
            if (nodes.ContainsKey(sourceId) && nodes.ContainsKey(targetId))
            {
                edges.Add((sourceId, targetId));
                nodes[sourceId].Neighbors.Add(nodes[targetId]);
                nodes[targetId].Neighbors.Add(nodes[sourceId]);
            }
        }

        public IEnumerable<GraphNode> GetNodes() => nodes.Values;
        public IEnumerable<(string Source, string Target)> GetEdges() => edges;
    }

    public class GraphNode
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public NodeType Type { get; set; }
        public object Data { get; set; }
        public List<GraphNode> Neighbors { get; set; } = new List<GraphNode>();

        public GraphNode(string id, string label, NodeType type, object data)
        {
            Id = id;
            Label = label;
            Type = type;
            Data = data;
            Neighbors = new List<GraphNode>();
        }
    }

    public enum NodeType
    {
        Category,
        IssueReport
    }
}
