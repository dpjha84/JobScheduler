using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Job Scheduler class
    /// </summary>
    public class JobSceduler
    {
        private readonly IInputParser _inputParser;
        private readonly IGraph _graph;

        public JobSceduler(IInputParser parser, IGraph graph)
        {
            _inputParser = parser;
            _graph = graph;
        }

        /// <summary>
        /// Schedules a given list of jobs (in a format) so that jobes are always executed first
        /// </summary>
        /// <param name="jobList">List of jobs where each entry is in the format: <job1> => <job2></param>
        /// <returns>Sequence of jobs such that dependencies are always executed first</returns>
        public string Schedule(List<string> jobList)
        {
            // Check for null and empty cases first
            if (jobList == null || jobList.Count == 0)
                return string.Empty;

            // Parse job list
            var jobEntries = _inputParser.Parse(jobList);

            // Build the graph by creating vertices and edges
            foreach (var jobEntry in jobEntries)
            {
                _graph.AddEdge(jobEntry);
            }            

            // Since Graph is built, get desired output now
            return _graph.GetSequence();
        }
    }
}
