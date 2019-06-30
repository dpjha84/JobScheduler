using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler.Validators
{
    public interface IInputValidator
    {
        /// <summary>
        /// Validates a JobEntry object
        /// </summary>
        /// <param name="jobEntry"></param>
        /// <returns></returns>
        bool Validate(JobEntry jobEntry);
    }
}
