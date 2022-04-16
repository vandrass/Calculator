using System;

namespace Calculator.Application.Exceptions
{
    public class DivisionByZeroExcepton : Exception
    {
        public DivisionByZeroExcepton()
        {
        }

        public DivisionByZeroExcepton(string message)
            : base(message)
        {
        }
    }
}
