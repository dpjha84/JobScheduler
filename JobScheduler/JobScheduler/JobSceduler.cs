using System;
using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Job Scheduler class
    /// </summary>
    public class JobSceduler
    {
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

            // Build the graph by creating vertices and edges after parsing job entries from job list
            var graph = new Graph();
            foreach (var jobEntry in jobList)
            {
                // Split each entry by "=>" and give the values to graph object
                var splittedJobs = jobEntry.Split(new string[] { "=>" }, StringSplitOptions.None);
                graph.AddEdge(splittedJobs[0], splittedJobs[1]);
            }

            // Since Graph is built, get desired output now
            return graph.GetSequence();
        }
    }
}
