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

            Calculating();

            if (enumErrors == EnumErrors.Success)
            {
                result = numbers[0];
            }

            return enumErrors;
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
            if (enumErrors == EnumErrors.Success)
            {
                return expression + "=" + numbers[0];
            }
            else if (enumErrors == EnumErrors.DivisionByZero)
            {
                return expression + " = " + "Division By Zero!";
            }
            else if (enumErrors == EnumErrors.NotCorrectExpression)
            {
                return expression + " = " + "Expression isn`t Correct!";
            }
            else if (enumErrors == EnumErrors.NoOperators)
            {
                return expression + " = " + "Operators Error!";
            }

            return "Empty String";
        }

        private void Calculating()
        {
            double result;

            CheckExpressionCorrection();
            if (enumErrors != EnumErrors.Correct)
            {
                return;
            }

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

        private void CheckExpressionCorrection()
        {
            if (enumErrors == EnumErrors.None)
            {
                if (numbers.Count < 1)
                {
                    enumErrors = EnumErrors.None;
                }
                else if (operators.Count == 0 || operators.Count != numbers.Count - 1)
                {
                    enumErrors = EnumErrors.NoOperators;
                }
                else
                {
                    enumErrors = EnumErrors.Correct;
                }
            }
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
            int operatorsCounter = 0;

            for (int i = 0; i < stringLenth; i++)
            {
                if (IsDigit(expression[i]))
                {
                    if (operatorsCounter == 1 && (char)operators[0] == '-' && i == 1)
                    {
                        strBuilder.Append('-');
                        strBuilder.Append(expression[i]);
                        operators.RemoveAt(operators.Count - 1);
                    }
                    else if (operatorsCounter == 2)
                    {
                        strBuilder.Append('-');
                        strBuilder.Append(expression[i]);
                        operators.RemoveAt(operators.Count - 1);
                    }
                    else
                    {
                        strBuilder.Append(expression[i]);
                    }

                    operatorsCounter = 0;
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

                        operatorsCounter++;

                        if (operatorsCounter == 2 && expression[i] != '-')
                        {
                            operatorsCounter = 0;
                        }
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
            if (strBuilder.Length > 0)
            {
                if (double.TryParse(strBuilder.ToString().Trim(), out double parseNumber))
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

        private void ResetObjectFields()
        {
            numbers.Clear();
            operators.Clear();
            enumErrors = EnumErrors.None;
        }

        private bool IsDigit(char ch)
        {
            if (ch != '+' && ch != '-' && ch != '*' && ch != '/' && ch != ' ' && ch != '(' && ch != ')')
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
