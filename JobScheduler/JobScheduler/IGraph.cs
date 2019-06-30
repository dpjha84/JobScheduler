namespace JobScheduler
{
    /// <summary>
    /// Interface IGraph
    /// </summary>
    public interface IGraph
    {
        /// <summary>
        /// Create vertices with source and destination name if not already, and create an edge between them
        /// </summary>
        /// <param name="source">Source vertex name</param>
        /// <param name="destination">Destination vertex name</param>
        void AddEdge(JobEntry jobEntry);

        /// <summary>
        /// Gets a sequence in which jobs (vertices) can be placed so that dependencies are always executed first
        /// </summary>
        /// <returns>Topologically sorted sequence of jobs (vertices)</returns>
        string GetSequence();
    }
}
