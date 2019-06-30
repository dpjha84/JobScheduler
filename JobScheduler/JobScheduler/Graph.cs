using JobScheduler.Exceptions;
using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Graph class which will hold all jobs (vertices) and their dependencies
    /// </summary>
    public class Graph : IGraph
    {
        private readonly Dictionary<string, Vertex> vertices = new Dictionary<string, Vertex>();

        /// <summary>
        /// Create vertices with source and destination name if not already, and create an edge between them
        /// </summary>
        /// <param name="source">Source vertex name</param>
        /// <param name="destination">Destination vertex name</param>
        public void AddEdge(JobEntry jobEntry)
        {
            var source = jobEntry.Name;
            var destination = jobEntry.DependsOnJobName;

            // Handle self dependency case
            if (source == destination)
                throw new SelfDependencyException("A job cannot be dependent on self. Job Name: " + source);

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
            // Check for cycle
            if (HasCycle())
                throw new CyclicDependencyException("Cycle found within jobs.");

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

        private bool HasCycle()
        {
            foreach (var node in vertices)
            {
                // As soon as cycle is detected, we return
                if (HasCycle(node.Value, new HashSet<string>(vertices.Count), null))
                    return true;
            }
            return false;
        }

        private bool HasCycle(Vertex node, HashSet<string> visited, Vertex previous)
        {
            // If we got a vertex which is already in visited set, it is a cycle
            if (visited.Contains(node.Name))
                return true;

            visited.Add(node.Name);
            foreach (var item in node.Edges)
            {
                // Ensure we don't recurse if this is same as previous vertex, because that will always be visited
                if (item.Value != previous)
                    return HasCycle(item.Value, visited, node);
            }
            return false;
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
