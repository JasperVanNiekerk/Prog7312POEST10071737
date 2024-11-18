using System;

namespace MinimumSpanningTreeLibrary
{
    public class Edge : IComparable<Edge>
    {
        public int Source { get; set; }
        public int Destination { get; set; }
        public int Weight { get; set; }
        //___________________________________________________________________________________________________________

        public Edge(int source, int destination, int weight)
        {
            Source = source;
            Destination = destination;
            Weight = weight;
        }
        //___________________________________________________________________________________________________________

        public int CompareTo(Edge other)
        {
            return Weight.CompareTo(other.Weight);
        }
    }
}
//____________________________________EOF_________________________________________________________________________