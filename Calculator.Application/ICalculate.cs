namespace Calculator.Application
{
    public interface ICalculate
    {
        public EnumErrors CalculateManualExpression(string expression, ref double result);

        public EnumErrors CalculateFileExpressions(string path);
    }
}
