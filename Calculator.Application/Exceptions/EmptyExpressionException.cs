using System;

namespace Calculator.Application.Exceptions
{
    public class EmptyExpressionException : Exception
    {
        public EmptyExpressionException()
        {
        }

        public EmptyExpressionException(string message)
            : base(message)
        {
        }
    }
}
