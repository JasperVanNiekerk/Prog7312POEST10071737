using System.Collections.Generic;

namespace MinimumSpanningTreeLibrary
{
    public class Graph
    {
        private readonly int _vertices;
        private readonly List<Edge> _edges;

        public Graph(int vertices)
        {
            _vertices = vertices;
            _edges = new List<Edge>();
        }
        //___________________________________________________________________________________________________________

        public void AddEdge(int source, int destination, int weight)
        {
            _edges.Add(new Edge(source, destination, weight));
        }
        //___________________________________________________________________________________________________________

        private int Find(int[] parent, int i)
        {
            if (parent[i] != i)
            {
                parent[i] = Find(parent, parent[i]); // Path compression
            }
            return parent[i];
        }
        //___________________________________________________________________________________________________________

        private void Union(int[] parent, int[] rank, int x, int y)
        {
            int xRoot = Find(parent, x);
            int yRoot = Find(parent, y);

            if (rank[xRoot] < rank[yRoot])
            {
                parent[xRoot] = yRoot;
            }
            else if (rank[xRoot] > rank[yRoot])
            {
                parent[yRoot] = xRoot;
            }
            else
            {
                parent[yRoot] = xRoot;
                rank[xRoot]++;
            }
        }
        //___________________________________________________________________________________________________________

        public (List<Edge> mstEdges, int totalWeight) KruskalMST()
        {
            List<Edge> result = new List<Edge>();
            int totalWeight = 0;

            // Sort edges by weight
            _edges.Sort();

            // Initialize parent and rank arrays
            int[] parent = new int[_vertices];
            int[] rank = new int[_vertices];

            for (int i = 0; i < _vertices; i++)
            {
                parent[i] = i;
                rank[i] = 0;
            }

            int edgeCount = 0;
            int index = 0;

            while (edgeCount < _vertices - 1 && index < _edges.Count)
            {
                Edge nextEdge = _edges[index++];

                int x = Find(parent, nextEdge.Source);
                int y = Find(parent, nextEdge.Destination);

                if (x != y)
                {
                    result.Add(nextEdge);
                    totalWeight += nextEdge.Weight;
                    Union(parent, rank, x, y);
                    edgeCount++;
                }
            }

            return (result, totalWeight);
        }
    }
}
//____________________________________EOF_________________________________________________________________________