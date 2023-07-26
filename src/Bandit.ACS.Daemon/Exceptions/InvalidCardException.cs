using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bandit.ACS.Exceptions
{
    [Serializable]
    public class InvalidCardException : Exception
    {
        public InvalidCardException() { }

        public InvalidCardException(string message) : base(message) { }
    }
}
