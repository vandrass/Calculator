using System;

namespace Calculator.Application.Exceptions
{
    public class NotCorrectExpressionException : Exception
    {
        public NotCorrectExpressionException()
        {
        }

        public NotCorrectExpressionException(string message)
            : base(message)
        {
        }

    }
}
