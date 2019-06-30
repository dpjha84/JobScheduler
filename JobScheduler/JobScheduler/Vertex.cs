using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Graph Vertex class
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// Gets unique name for each vertex.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets edges collection.
        /// </summary>
        public Dictionary<string, Vertex> Edges { get; }        

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="name"></param>
        public Vertex(string name)
        {
            Name = name;
            Edges = new Dictionary<string, Vertex>();
        }

        /// <summary>
        /// Add an edge from this to other vertex
        /// </summary>
        /// <param name="destination">Other vertex which needs to be connected</param>
        public void AddEdge(Vertex destination)
        {
            if(!Edges.ContainsKey(destination.Name))
                Edges.Add(destination.Name, destination);
        }        
    }
}
