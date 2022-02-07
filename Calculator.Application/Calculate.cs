﻿using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Application
{
    /// <summary>
    /// Class parse string expression from user or file and calculate them.
    /// </summary>
    public class Calculate : ICalculate
    {
        private EnumErrors enumErrors = EnumErrors.None;
        private List<double> numbers = new List<double>();
        private ArrayList operators = new ArrayList();

        /// <summary>
        /// Calculate expression string from user.
        /// </summary>
        /// <param name="expression">string from user.</param>
        /// <param name="result">resukt if calculation.</param>
        /// <returns>return errors enum.</returns>
        public EnumErrors CalculateManualExpression(string expression, ref double result)
        {
            ParseString(expression);

            if (enumErrors == EnumErrors.NotCorrectExpression)
            {
                return enumErrors;
            }

            if (operators.Count == 0 || operators.Count != numbers.Count - 1)
            {
                return EnumErrors.NoOperators;
            }

            Calculating();

            result = numbers[0];

            return enumErrors;
        }

        public EnumErrors CalculateFileExpressions(string path)
        {
            return enumErrors;
        }

        private void Calculating()
        {
            double result = 0;

            while (operators.Count > 0)
            {
                for (int i = 0; i < operators.Count; i++)
                {
                    if ((char)operators[i] == '/')
                    {
                        if (numbers[i + 1] == 0)
                        {
                            enumErrors = EnumErrors.DivisionByZero;
                            return;
                        }

                        result = Division(numbers[i], numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < operators.Count; i++)
                {
                    if ((char)operators[i] == '*')
                    {
                        result = Multiplication(numbers[i], numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < operators.Count; i++)
                {
                    if ((char)operators[i] == '+')
                    {
                        result = Sum(numbers[i], numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < operators.Count; i++)
                {
                    if ((char)operators[i] == '-')
                    {
                        result = Sub(numbers[i], numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }
            }

            enumErrors = EnumErrors.Success;
        }

        private void InsertAndRemove(double result, int i)
        {
            numbers.Insert(i, result);
            numbers.RemoveRange(i + 1, 2);
            operators.RemoveAt(i);
        }

        private void ParseString(string expression)
        {
            var stringLenth = expression.Length;
            var strBuilder = new StringBuilder();

            for (int i = 0; i < stringLenth; i++)
            {
                if (IsDigit(expression[i]))
                {
                    strBuilder.Append(expression[i]);
                }
                else
                {
                    if (!ParseNumber(strBuilder))
                    {
                        if (enumErrors == EnumErrors.NotCorrectExpression)
                        {
                            return;
                        }
                    }

                    if (i != stringLenth - 1 && expression[i] != ' ')
                    {
                        operators.Add(expression[i]);
                    }
                }

                if (i == stringLenth - 1)
                {
                    ParseNumber(strBuilder);
                }
            }
        }

        private bool ParseNumber(StringBuilder strBuilder)
        {
            double parseNumber;

            if (strBuilder.Length > 0)
            {
                if (double.TryParse(strBuilder.ToString().Trim(), out parseNumber))
                {
                    numbers.Add(parseNumber);

                    strBuilder.Clear();

                    return true;
                }

                enumErrors = EnumErrors.NotCorrectExpression;

                return false;
            }

            return false;
        }

        private bool IsDigit(char ch)
        {
            if (ch != '+' && ch != '-' && ch != '*' && ch != '/' && ch != ' ')
            {
                return true;
            }

            return false;
        }

        private double Sum(double a, double b)
        {
            return a + b;
        }

        private double Sub(double a, double b)
        {
            return a - b;
        }

        private double Division(double a, double b)
        {
            return a / b;
        }

        private double Multiplication(double a, double b)
        {
            return b * a;
        }
    }
}
