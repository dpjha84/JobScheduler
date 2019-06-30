using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Interface for Input Parser
    /// </summary>
    public interface IInputParser
    {
        /// <summary>
        /// Parses input list of string and returns list of JobEntry
        /// </summary>
        /// <param name="jobList"></param>
        /// <returns>List of JobEntry</returns>
        IList<JobEntry> Parse(IList<string> jobList);
    }
}
