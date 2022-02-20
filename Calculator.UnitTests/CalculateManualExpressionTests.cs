using Calculator.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.UnitTests
{
    /// <summary>
    /// Tests for CalculateManualExpression method.
    /// </summary>
    [TestClass]
    public class CalculateManualExpressionTests
    {
        private readonly ServiceCollection _serviceCollection = new ();
        private ServiceProvider _serviceProvider;
        private ICalculate _service;

        /// <summary>
        /// Service collection for working with Calculate class.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            _serviceCollection.AddScoped<ICalculate, Calculate>();
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            _service = _serviceProvider.GetRequiredService<ICalculate>();
        }

        /// <summary>
        /// Check string exepression with correct input and correct result of exeption.
        /// </summary>
        [TestMethod]
        public void StringExpression_CorrectExpression_CorrectResult()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5";
            var expected = -3;

            // Act
            var actual = 0.0;
            _service.CalculateManualExpression(expression, ref actual);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string exepression with correct input and correct result of exeption.
        /// </summary>
        [TestMethod]
        public void StringExpression_FirstCharIsMinus_CorrectResult()
        {
            // Arrange
            var expression = "-1 + 2 - 3 * 10 / 5";
            var expected = -5;

            // Act
            var actual = 0.0;
            _service.CalculateManualExpression(expression, ref actual);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with correct input and correct result, EnumErrors will be "success" .
        /// </summary>
        [TestMethod]
        public void StringExpression_InCorrectExpression_EnumNoOperators()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5-";
            var expected = EnumErrors.NoOperators;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with correct input and correct result, EnumErrors will be "success" .
        /// </summary>
        [TestMethod]
        public void StringExpression_CorrectExpression_EnumSuccess()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5";
            var expected = EnumErrors.Success;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with division by zero, EnumErrors will be "divisionByZero" .
        /// </summary>
        [TestMethod]
        public void StringExpression_DivByZero_EnumDivisionByZero()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 0";
            var expected = EnumErrors.DivisionByZero;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with not correct input, EnumErrors will be "NotCorrectExpression" .
        /// </summary>
        [TestMethod]
        public void StringExpression_NotCorrectInput_EnumNotCorrectExpression()
        {
            // Arrange
            var expression = "1a + 2 - 3 * 10 / er";
            var expected = EnumErrors.NotCorrectExpression;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression without enough  operators, EnumErrors will be "NoOperators" .
        /// </summary>
        [TestMethod]
        public void StringExpression_NotEnoughOperators_EnumNoOperators()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10   9";
            var expected = EnumErrors.NoOperators;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with empty input, EnumErrors will be "None" .
        /// </summary>
        [TestMethod]
        public void StringExpression_EmptyString_EnumNone()
        {
            // Arrange
            var expression = " ";
            var expected = EnumErrors.None;
            var result = 0.0;

            // Act
            var actual = _service.CalculateManualExpression(expression, ref result);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
