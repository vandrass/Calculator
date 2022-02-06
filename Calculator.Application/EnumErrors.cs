﻿namespace Calculator.Application
{
    /// <summary>
    /// Sign user input errors for calculator.
    /// </summary>
    public enum EnumErrors
    {
        /// <summary>
        /// If is division by zero.
        /// </summary>
        DivisionByZero,

        /// <summary>
        /// If No Math Operators.
        /// </summary>
        NoOperators,

        /// <summary>
        /// If there are not correct symbols(e.g. letters ).
        /// </summary>
        NotCorrectExpression,

        /// <summary>
        /// If expression is correct.
        /// </summary>
        Success,
    }
}