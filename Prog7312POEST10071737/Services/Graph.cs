using System.Collections.Generic;

namespace Prog7312POEST10071737.Services
{
    internal class Graph
    {
        private Dictionary<string, GraphNode> nodes;
        private List<(string Source, string Target)> edges;
        //___________________________________________________________________________________________________________

        public Graph()
        {
            nodes = new Dictionary<string, GraphNode>();
            edges = new List<(string Source, string Target)>();
        }
        //___________________________________________________________________________________________________________

        public void AddNode(string id, string label, NodeType type, object data)
        {
            if (!nodes.ContainsKey(id))
            {
                nodes[id] = new GraphNode(id, label, type, data);
            }
        }
        //___________________________________________________________________________________________________________

        public void AddEdge(string sourceId, string targetId)
        {
            if (nodes.ContainsKey(sourceId) && nodes.ContainsKey(targetId))
            {
                edges.Add((sourceId, targetId));
                nodes[sourceId].Neighbors.Add(nodes[targetId]);
                nodes[targetId].Neighbors.Add(nodes[sourceId]);
            }
        }
        //___________________________________________________________________________________________________________

        public IEnumerable<GraphNode> GetNodes() => nodes.Values;
        public IEnumerable<(string Source, string Target)> GetEdges() => edges;
    }
    //___________________________________________________________________________________________________________

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
    //___________________________________________________________________________________________________________

    public enum NodeType
    {
        Category,
        IssueReport
    }
}
//____________________________________EOF_________________________________________________________________________