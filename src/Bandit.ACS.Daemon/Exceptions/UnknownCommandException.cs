using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class UnknownCommandException : Exception
    {
        public UnknownCommandException() { }

        public UnknownCommandException(string message) : base(message) { }
    }
}
