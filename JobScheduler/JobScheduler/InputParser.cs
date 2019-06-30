using JobScheduler.Validators;
using System;
using System.Collections.Generic;

namespace JobScheduler
{
    /// <summary>
    /// Input Parser
    /// </summary>
    public class InputParser : IInputParser
    {
        private readonly IInputValidator _validator;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="validator"></param>
        public InputParser(IInputValidator validator)
        {
            _validator = validator;
        }

        /// <summary>
        /// Parses input list of string and returns list of JobEntry
        /// </summary>
        /// <param name="jobList"></param>
        /// <returns>List of JobEntry</returns>
        public IList<JobEntry> Parse(IList<string> jobList)
        {
            var result = new List<JobEntry>();
            foreach (var entry in jobList)
            {
                if (!entry.Contains("=>"))
                    throw new InvalidOperationException($"Invalid format: {entry}");

                var splittedJobs = entry.Split(new string[] { "=>" }, StringSplitOptions.None);
                var jobEntry = new JobEntry
                {
                    Name = splittedJobs[0].Trim(),
                    DependsOnJobName = splittedJobs[1].Trim()
                };
                // Validate
                _validator.Validate(jobEntry);
                result.Add(jobEntry);
            }
            return result;
        }
    }
}
