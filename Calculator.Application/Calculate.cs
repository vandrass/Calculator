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
        private ArrayList _inputString = new ArrayList();
        private ArrayList _outputString = new ArrayList();
        private int _numbersCount = 0;
        private int _operatorsCount = 0;

        private List<double> _numbers = new List<double>();
        private ArrayList _operators = new ArrayList();
        private bool _withParentheses;
        private Dictionary<char, int> _operatorsPriority = new Dictionary<char, int>()
        {
            ['('] = 5,
            [')'] = 5,
            ['*'] = 4,
            ['/'] = 4,
            ['-'] = 3,
            ['+'] = 3,
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

            Calculating();

            if (_enumErrors == EnumErrors.Success)
            {
                result = _numbers[0];
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
                _withParentheses = true;
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
                return expression + "=" + _numbers[0];
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

            for (int i = 0; i < stringLenth; i++)
            {
                if (char.IsDigit(expression[i]))
                {
                    _inputString.Add((double)expression[i]);
                    _numbersCount++;
                }
                else
                {
                    ParseOperator(expression[i]);
                }
            }
        }

        private void ParseOperator(char oper)
        {
            if (oper == '(' || oper == ')' || oper == '/' || oper == '*' || oper == '-' || oper == '+')
            {
                _inputString.Add(oper);
                if (oper != '(' || oper != ')')
                {
                    _operatorsCount++;
                }
            }
            else if (oper != ' ')
            {
                _enumErrors = EnumErrors.NotCorrectExpression;
            }
        }

        private void Calculating()
        {
            double result;

            CheckExpressionCorrection();
            if (_enumErrors != EnumErrors.Correct)
            {
                return;
            }

            while (_operators.Count > 0)
            {
                for (int i = 0; i < _operators.Count; i++)
                {
                    if ((char)_operators[i] == '/')
                    {
                        if (_numbers[i + 1] == 0)
                        {
                            _enumErrors = EnumErrors.DivisionByZero;
                            return;
                        }

                        result = Division(_numbers[i], _numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < _operators.Count; i++)
                {
                    if ((char)_operators[i] == '*')
                    {
                        result = Multiplication(_numbers[i], _numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < _operators.Count; i++)
                {
                    if ((char)_operators[i] == '+')
                    {
                        result = Sum(_numbers[i], _numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }

                for (int i = 0; i < _operators.Count; i++)
                {
                    if ((char)_operators[i] == '-')
                    {
                        result = Sub(_numbers[i], _numbers[i + 1]);
                        InsertAndRemove(result, i);
                        i--;
                    }
                }
            }

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
                else
                {
                    _enumErrors = EnumErrors.Correct;
                }
            }
        }

        private void InsertAndRemove(double result, int i)
        {
            _numbers.Insert(i, result);
            _numbers.RemoveRange(i + 1, 2);
            _operators.RemoveAt(i);
        }

        private void ResetObjectFields()
        {
            _numbers.Clear();
            _operators.Clear();
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
