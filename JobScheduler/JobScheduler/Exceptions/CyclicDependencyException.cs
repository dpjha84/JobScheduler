using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduler.Exceptions
{
    public class CyclicDependencyException : Exception
    {
        public CyclicDependencyException(string message) : base (message)
        {

        }
    }
}
