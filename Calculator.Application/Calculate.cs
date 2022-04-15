using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Calculator.Application.Exceptions;

namespace Calculator.Application
{
    /// <summary>
    /// Class parse string expression from user or file and calculate them.
    /// </summary>
    public class Calculate : ICalculate
    {
        private ArrayList _inputArray = new ArrayList();
        private ArrayList _outputArray = new ArrayList();
        private Stack _operationsStack = new Stack();
        private int _numbersCount = 0;
        private int _operatorsCount = 0;
        private int _openBraces = 0;
        private int _closeBraces = 0;
        private double _result;
        private IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        private Dictionary<char, int> _operatorsPriority = new Dictionary<char, int>()
        {
            ['*'] = 4,
            ['/'] = 4,
            ['-'] = 3,
            ['+'] = 2,
            ['('] = 1,
            [')'] = 1,
        };

        /// <summary>
        /// Method calculate sum of a and b.
        /// </summary>
        /// <param name="a">first number.</param>
        /// <param name="b">second number.</param>
        /// <returns>result of sum of a and b.</returns>
        public static double Sum(double a, double b)
        {
            return a + b;
        }

        /// <summary>
        /// Method calculate subtraction of b from a.
        /// </summary>
        /// <param name="a">first number.</param>
        /// <param name="b">second number.</param>
        /// <returns>result of subtraction of b from a.</returns>
        public static double Sub(double a, double b)
        {
            return a - b;
        }

        /// <summary>
        /// Method calculate division of a by b.
        /// </summary>
        /// <param name="a">first number.</param>
        /// <param name="b">second number.</param>
        /// <returns>result of division of a by b.</returns>
        public static double Division(double a, double b)
        {
            return a / b;
        }

        /// <summary>
        /// Method calculate multiplication of a on b.
        /// </summary>
        /// <param name="a">first number.</param>
        /// <param name="b">second number.</param>
        /// <returns>result multiplication of of a on b.</returns>
        public static double Multiplication(double a, double b)
        {
            return b * a;
        }

        /// <summary>
        /// Calculate expression string from user.
        /// </summary>
        /// <param name="expression">string from user.</param>
        /// <returns>result of calculation.</returns>
        public double CalculateManualExpression(string expression)
        {
            ParseString(expression);
            CheckExpressionCorrection();

            BuildOutputArray();
            Calculating();

            return _result;
        }

        /// <summary>
        /// Read file, calculate expressions from him and save expressions with ansewrs to new file.
        /// </summary>
        /// <param name="path">path to file for reading.</param>
        /// <returns>true - if file saved successfully, false - if file was not saved.</returns>
        public bool CalculateFileExpressions(string path)
        {
            var expressionsList = GetExpressionsFromFile(path);

            foreach (var expression in expressionsList)
            {
                try
                {
                    ParseString(expression);
                    CheckExpressionCorrection();
                    BuildOutputArray();
                    Calculating();
                    WriteAnswerToNewFile(expression + " = " + _result, path);
                }
                catch (Exception e)
                {
                    WriteAnswerToNewFile(expression + " = " + e.Message, path);
                }

                ResetObjectFields();
            }

            return true;
        }

        private List<string> GetExpressionsFromFile(string path)
        {
            var expressionsList = new List<string>();

            foreach (string line in File.ReadLines(path))
            {
                expressionsList.Add(line);
            }

            return expressionsList;
        }

        private void WriteAnswerToNewFile(string expression, string path)
        {
            string answerFilePath = path + "_answer.txt";
            if (!File.Exists(answerFilePath))
            {
                using var streamWriter = new StreamWriter(answerFilePath);
                {
                    streamWriter.WriteLine(expression);
                }
            }
            else
            {
                using var streamWriter = new StreamWriter(answerFilePath, append: true);
                {
                    streamWriter.WriteLine(expression);
                }
            }
        }

        private void ParseString(string expression)
        {
            var stringLenth = expression.Length;
            var strBuilder = new StringBuilder();

            for (int i = 0; i < stringLenth; i++)
            {
                if (char.IsDigit(expression[i]) || (i == 0 && expression[i] == '-') || expression[i] == '.')
                {
                    strBuilder.Append(expression[i]);

                    if (i == stringLenth - 1)
                    {
                        _inputArray.Add(double.Parse(strBuilder.ToString(), formatter));
                        _numbersCount++;
                        strBuilder.Clear();
                    }
                }
                else
                {
                    if (strBuilder.Length > 0)
                    {
                        _inputArray.Add(double.Parse(strBuilder.ToString(), formatter));
                        _numbersCount++;
                        strBuilder.Clear();
                    }

                    ParseOperator(expression[i]);
                }
            }
        }

        private void ParseOperator(char operand)
        {
            if (operand == '(' || operand == ')' || operand == '/' || operand == '*' || operand == '-' || operand == '+')
            {
                _inputArray.Add(operand);

                if (operand != '(' && operand != ')')
                {
                    _operatorsCount++;
                }
                else if (operand == '(')
                {
                    _openBraces++;
                }
                else
                {
                    _closeBraces++;
                }
            }
            else if (operand != ' ')
            {
                throw new NotCorrectExpressionException("Not Correct Expression!");
            }
        }

        private void Calculating()
        {
            var numbers = new Stack<double>();

            for (int i = 0; i < _outputArray.Count; i++)
            {
                Type t = _outputArray[i].GetType();

                if (t.Equals(typeof(double)))
                {
                    numbers.Push((double)_outputArray[i]);
                }
                else
                {
                    double secondNumber = numbers.Pop();
                    double firstNumber = numbers.Pop();

                    if ((char)_outputArray[i] == '+')
                    {
                        numbers.Push(Sum(firstNumber, secondNumber));
                    }
                    else if ((char)_outputArray[i] == '-')
                    {
                        numbers.Push(Sub(firstNumber, secondNumber));
                    }
                    else if ((char)_outputArray[i] == '*')
                    {
                        numbers.Push(Multiplication(firstNumber, secondNumber));
                    }
                    else if ((char)_outputArray[i] == '/')
                    {
                        if (secondNumber == 0)
                        {
                            throw new DivisionByZeroExcepton("Division By Zero!");
                        }

                        numbers.Push(Division(firstNumber, secondNumber));
                    }
                }
            }

            _result = numbers.Peek();
        }

        private void CheckExpressionCorrection()
        {
            if (_numbersCount < 1)
            {
                throw new EmptyExpressionException("Empty Expression!");
            }

            if (_operatorsCount == 0 || _operatorsCount != _numbersCount - 1)
            {
                throw new OperatorErrorException("No correct operators number!");
            }

            if (_openBraces != _closeBraces)
            {
                throw new NotCorrectExpressionException("Not Correct Expression!");
            }
        }

        private void BuildOutputArray()
        {
            var priority = _operatorsPriority;
            var stack = _operationsStack;

            for (var i = 0; i < _inputArray.Count; i++)
            {
                Type t = _inputArray[i].GetType();

                if (t.Equals(typeof(double)))
                {
                    _outputArray.Add(_inputArray[i]);
                }
                else
                {
                    if ((char)_inputArray[i] == '(')
                    {
                        stack.Push(_inputArray[i]);
                    }
                    else if ((char)_inputArray[i] == ')')
                    {
                        char temp = (char)stack.Pop();

                        while (temp != '(')
                        {
                            _outputArray.Add(temp);
                            temp = (char)stack.Pop();
                        }
                    }
                    else if (stack.Count < 1)
                    {
                        stack.Push(_inputArray[i]);
                    }
                    else
                    {
                        if (priority[(char)stack.Peek()] >= priority[(char)_inputArray[i]])
                        {
                            _outputArray.Add(stack.Pop());
                            while (stack.Count != 0 && (char)stack.Peek() == '-' && priority[(char)stack.Peek()] >= priority[(char)_inputArray[i]])
                            {
                                _outputArray.Add(stack.Pop());
                            }

                            stack.Push(_inputArray[i]);
                        }
                        else
                        {
                            stack.Push(_inputArray[i]);
                        }
                    }
                }

                if (i == _inputArray.Count - 1)
                {
                    while (stack.Count > 0)
                    {
                            _outputArray.Add(stack.Pop());
                    }
                }
            }
        }

        private void ResetObjectFields()
        {
            _outputArray.Clear();
            _operationsStack.Clear();
            _inputArray.Clear();
            _numbersCount = 0;
            _operatorsCount = 0;
            _openBraces = 0;
            _closeBraces = 0;
            _result = 0;
        }
    }
}
