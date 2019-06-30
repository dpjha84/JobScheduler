using System;
using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Graph class which will hold all jobs (vertices) and their dependencies
    /// </summary>
    public class Graph
    {
        private readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();

        /// <summary>
        /// Create vertices with source and destination name if not already, and create an edge between them
        /// </summary>
        /// <param name="source">Source vertex name</param>
        /// <param name="destination">Destination vertex name</param>
        public void AddEdge(string source, string destination)
        {
            // Ensure there is no leading or trailing spaces in name
            source = source.Trim();
            destination = destination.Trim();            

            // Create vertex objects first if not already created
            if (!vertices.ContainsKey(source))
            {
                vertices.Add(source, new Vertex(source));
            }

            // No need to do anything if destination is null or empty
            if (string.IsNullOrWhiteSpace(destination)) return;

            if (!vertices.ContainsKey(destination))
            {
                vertices.Add(destination, new Vertex(destination));
            }

            // Connect both vertices
            vertices[source].AddEdge(vertices[destination]);
        }

        /// <summary>
        /// Gets a sequence in which jobs (vertices) can be placed so that dependencies are always executed first
        /// </summary>
        /// <returns>Topologically sorted sequence of jobs (vertices)</returns>
        public string GetSequence()
        {
            var topologicalSortedList = new List<string>(vertices.Count);
            var visited = new HashSet<string>(vertices.Count);

            // Do a topological sort
            foreach (var vertex in vertices)
            {
                if (!visited.Contains(vertex.Value.Name))
                    TopologicalSort(vertex.Value, visited, topologicalSortedList);
            }

            // Once we got the topologically sorted vertices in list, join them to get the string
            return string.Join("", topologicalSortedList);
        }

        private void TopologicalSort(Vertex current, HashSet<string> visited, List<string> list)
        {
            // If vertex is already visited return, else add to visited set
            if (visited.Contains(current.Name))
                return;
            visited.Add(current.Name);

            // Recurse for each neighbour node of current vertex
            foreach (var item in current.Edges)
            {
                TopologicalSort(item.Value, visited, list);
            }

            // Once current vertex is done, add it to the list
            if (!list.Contains(current.Name))
                list.Add(current.Name);
        }
    }    
}
