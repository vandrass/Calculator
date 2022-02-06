namespace Calculator.Application
{
    public interface ICalculate
    {
        public EnumErrors CalculateManualExpression();

        public EnumErrors CalculateFileExpressions();
    }
}
