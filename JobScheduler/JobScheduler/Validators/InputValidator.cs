using JobScheduler.Exceptions;

namespace JobScheduler.Validators
{
    /// <summary>
    /// Validator to validate JobEntry object
    /// </summary>
    public class InputValidator : IInputValidator
    {
        /// <summary>
        /// Validates a JobEntry object
        /// </summary>
        /// <param name="jobEntry"></param>
        /// <returns></returns>
        public bool Validate(JobEntry jobEntry)
        {
            if(jobEntry.Name.Length != 1)
                throw new InvalidJobNameException($"Invalid job name: {jobEntry.Name}");
            if (jobEntry.DependsOnJobName.Length > 1)
                throw new InvalidJobNameException($"Invalid job name: {jobEntry.DependsOnJobName}");
            return true;
        }
    }
}
