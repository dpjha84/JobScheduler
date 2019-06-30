using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler.Exceptions
{
    public class SelfDependencyException : Exception
    {
        public SelfDependencyException(string message) : base (message)
        {

        }
    }
}
