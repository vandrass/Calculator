using System.Collections;
using System.Text;

namespace Calculator.Application
{
    public class Calculate : ICalculate
    {
        private EnumErrors enumErrors = EnumErrors.None;
        private ArrayList numbers = new ArrayList();
        private ArrayList operators = new ArrayList();

        public EnumErrors CalculateManualExpression(string expression)
        {
            ParseString(expression);

            if (enumErrors == EnumErrors.NotCorrectExpression)
            {
                return enumErrors;
            }

            if (operators.Count == 0)
            {
                return EnumErrors.NoOperators;
            }

            Calculating();

            return enumErrors;
        }

        public EnumErrors CalculateFileExpressions(string path)
        {
            return enumErrors;
        }

        private void Calculating()
        {
            double result = 0;

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

                    if (i != stringLenth - 1)
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
            if (ch != '+' && ch != '-' && ch != '*' && ch != '/')
            {
                return true;
            }

            return false;
        }

        private void Sum()
        {

        }

        private void Sub()
        {

        }

        private void Division()
        {

        }

        private void Multiplication()
        {

        }
    }
}
