namespace Calculator.Application
{
    /// <summary>
    /// Interface for Calculating methods implementation.
    /// </summary>
    public interface ICalculate
    {
        /// <summary>
        /// Calculate expression string from user.
        /// </summary>
        /// <param name="expression">string from user.</param>
        /// <returns>result of calculation.</returns>
        public double CalculateManualExpression(string expression);

        /// <summary>
        /// Read file, calculate expressions from him and save expressions with ansewrs to new file.
        /// </summary>
        /// <param name="path">path to file for reading.</param>
        /// <returns>true - if file saved successfully, false - if file was not saved.</returns>
        public bool CalculateFileExpressions(string path);
    }
}
