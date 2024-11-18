using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimumSpanningTreeLibrary
{
    public class Class1
    {
        public static void Main(string[] args)
        {
            Graph graph = new Graph(4);

            // Add edges (source, destination, weight)
            graph.AddEdge(0, 1, 10);
            graph.AddEdge(0, 2, 6);
            graph.AddEdge(0, 3, 5);
            graph.AddEdge(1, 3, 15);
            graph.AddEdge(2, 3, 4);

            // Find MST
            var (mstEdges, totalWeight) = graph.KruskalMST();

            Console.WriteLine("Edges in the Minimum Spanning Tree:");
            foreach (var edge in mstEdges)
            {
                Console.WriteLine($"{edge.Source} -- {edge.Destination} == {edge.Weight}");
            }
            Console.WriteLine($"Total weight of MST: {totalWeight}");
        }
    }
}
//____________________________________EOF_________________________________________________________________________