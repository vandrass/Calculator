using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Application.Exceptions
{
    public class OperatorErrorException : Exception
    {
        public OperatorErrorException()
        {
        }

        public OperatorErrorException(string message)
            : base(message)
        {
        }
    }
}
