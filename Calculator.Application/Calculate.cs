using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calculator.Application
{
    /// <summary>
    /// Class parse string expression from user or file and calculate them.
    /// </summary>
    public class Calculate : ICalculate
    {
        private EnumErrors _enumErrors = EnumErrors.None;
        private ArrayList _inputArray = new ArrayList();
        private ArrayList _outputArray = new ArrayList();
        private Stack _operationsStack = new Stack();
        private int _numbersCount = 0;
        private int _operatorsCount = 0;
        private int _openBraces = 0;
        private int _closeBraces = 0;
        private double _result;
        private Dictionary<char, int> _operatorsPriority = new Dictionary<char, int>()
        {
            ['*'] = 4,
            ['/'] = 4,
            ['-'] = 3,
            ['+'] = 3,
            ['('] = 2,
            [')'] = 2,
        };

        /// <summary>
        /// Calculate expression string from user.
        /// </summary>
        /// <param name="expression">string from user.</param>
        /// <param name="result">resukt if calculation.</param>
        /// <returns>return errors enum.</returns>
        public EnumErrors CalculateManualExpression(string expression, ref double result)
        {
            ParseString(expression);
            CheckExpressionCorrection();

            if (_enumErrors == EnumErrors.Correct)
            {
                BuildOutputArray();

                foreach (var output in _outputArray)
                {
                    Console.WriteLine(output);
                }

                Calculating();
            }

            if (_enumErrors == EnumErrors.Success)
            {
                result = _result;
            }

            return _enumErrors;
        }

        /// <summary>
        /// Read file, calculate expressions from him and save expressions with ansewrs to new file.
        /// </summary>
        /// <param name="path">path to file for reading.</param>
        /// <returns>true - if file saved successfully, false - if file was not saved.</returns>
        public bool CalculateFileExpressions(string path)
        {
            List<string> expressionsList = GetExpressionsFromFile(path);

            foreach (var expression in expressionsList)
            {
                ParseString(expression);
                Calculating();
                WriteAnswerToNewFile(expression, path);
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
            string answerFilePath = path + "answer.txt";
            if (!File.Exists(answerFilePath))
            {
                using var streamWriter = new StreamWriter(answerFilePath);
                {
                    streamWriter.WriteLine(GetRightAnswer(expression));
                }
            }
            else
            {
                using var streamWriter = new StreamWriter(answerFilePath, append: true);
                {
                    streamWriter.WriteLine(GetRightAnswer(expression));
                }
            }
        }

        private string GetRightAnswer(string expression)
        {
            if (_enumErrors == EnumErrors.Success)
            {
                return expression + "=" + _result;
            }
            else if (_enumErrors == EnumErrors.DivisionByZero)
            {
                return expression + " = " + "Division By Zero!";
            }
            else if (_enumErrors == EnumErrors.NotCorrectExpression)
            {
                return expression + " = " + "Expression isn`t Correct!";
            }
            else if (_enumErrors == EnumErrors.OperatorsError)
            {
                return expression + " = " + "Operators Error!";
            }

            return "Empty String";
        }

        private void ParseString(string expression)
        {
            var stringLenth = expression.Length;
            var strBuilder = new StringBuilder();

            for (int i = 0; i < stringLenth; i++)
            {
                if (char.IsDigit(expression[i]) || (i == 0 && expression[i] == '-'))
                {
                    strBuilder.Append(expression[i]);

                    if (i == stringLenth - 1)
                    {
                        _inputArray.Add(double.Parse(strBuilder.ToString()));
                        _numbersCount++;
                        strBuilder.Clear();
                    }
                }
                else
                {
                    if (strBuilder.Length > 0)
                    {
                        _inputArray.Add(double.Parse(strBuilder.ToString()));
                        _numbersCount++;
                        strBuilder.Clear();
                    }

                    ParseOperator(expression[i]);
                }
            }
        }

        private void ParseOperator(char oper)
        {
            if (oper == '(' || oper == ')' || oper == '/' || oper == '*' || oper == '-' || oper == '+')
            {
                _inputArray.Add(oper);

                if (oper != '(' || oper != ')')
                {
                    _operatorsCount++;
                }
                else if (oper == '(')
                {
                    _openBraces++;
                }
                else
                {
                    _closeBraces++;
                }
            }
            else if (oper != ' ')
            {
                _enumErrors = EnumErrors.NotCorrectExpression;
            }
        }

        private void Calculating()
        {
            Stack<double> numbers = new Stack<double>();

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
                            _enumErrors = EnumErrors.DivisionByZero;
                            return;
                        }

                        numbers.Push(Division(firstNumber, secondNumber));
                    }
                }
            }

            _result = numbers.Peek();

            _enumErrors = EnumErrors.Success;
        }

        private void CheckExpressionCorrection()
        {
            if (_enumErrors == EnumErrors.None)
            {
                if (_numbersCount < 1)
                {
                    _enumErrors = EnumErrors.None;
                }
                else if (_operatorsCount == 0 || _operatorsCount != _numbersCount - 1)
                {
                    _enumErrors = EnumErrors.OperatorsError;
                }
                else if (_openBraces != _closeBraces)
                {
                    _enumErrors = EnumErrors.NotCorrectExpression;
                }
                else
                {
                    _enumErrors = EnumErrors.Correct;
                }
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
            _enumErrors = EnumErrors.None;
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
