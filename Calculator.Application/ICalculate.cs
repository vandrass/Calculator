namespace Calculator.Application
{
    public interface ICalculate
    {
        public EnumErrors CalculateManualExpression(string expression);

        public EnumErrors CalculateFileExpressions(string path);
    }
}
