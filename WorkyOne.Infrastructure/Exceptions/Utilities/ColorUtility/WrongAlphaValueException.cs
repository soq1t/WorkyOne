using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkyOne.Infrastructure.Exceptions.Utilities.ColorUtility
{
    public class WrongAlphaValueException : Exception
    {
        public WrongAlphaValueException()
            : base("The alpha must be in range 0 - 1") { }
    }
}
