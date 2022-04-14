using Calculator.Application;
using Calculator.Application.Exceptions;
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
        /// Check string exepression with correct input and correct result of expression.
        /// </summary>
        [TestMethod]
        public void StringExpression_CorrectExpression_CorrectResult()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5";
            var expected = -3;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string exepression with correct input and correct result of expression.
        /// </summary>
        [TestMethod]
        public void StringExpression_CorrectExpressionOne_CorrectResult()
        {
            // Arrange
            var expression = "5-5/5+5";
            var expected = 9;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string exepression with correct input and correct result of expression.
        /// </summary>
        [TestMethod]
        public void StringExpression_FirstCharIsMinus_CorrectResult()
        {
            // Arrange
            var expression = "-1 + 2 - 3 * 10 / 5";
            var expected = -5;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression enough  operators and enough braces, right result of calculating .
        /// </summary>
        [TestMethod]
        public void StringExpression_EnoughOperatorsAndBraces_RightResult()
        {
            // Arrange
            var expression = "((1 + 2) - 3) * 10";
            var expected = 0;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression enough  operators and enough braces and with floating point numbers, right result of calculating .
        /// </summary>
        [TestMethod]
        public void StringExpression_EnoughOperatorsAndBracesAndFloatingPointNumber_RightResult()
        {
            // Arrange
            var expression = "((10-5)/0.5*(1+4)*0.001)";
            var expected = 0.05;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Check string expression with correct input and correct result, should throw OperatorErrorException" .
        /// </summary>
        [TestMethod]
        public void StringExpression_InCorrectExpression_EnumNoOperators()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5-";

            // Assert
            Assert.ThrowsException<OperatorErrorException>(() => _service.CalculateManualExpression(expression));
        }

        /// <summary>
        /// Check string expression with division by zero, should throw DivisionByZeroExcepton .
        /// </summary>
        [TestMethod]
        public void StringExpression_DivByZero_EnumDivisionByZero()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 0";

            // Assert
            Assert.ThrowsException<DivisionByZeroExcepton>(() => _service.CalculateManualExpression(expression));
        }

        /// <summary>
        /// Check string expression with not correct input, should throw NotCorrectExpressionException.
        /// </summary>
        [TestMethod]
        public void StringExpression_NotCorrectInput_EnumNotCorrectExpression()
        {
            // Arrange
            var expression = "1a + 2 - 3 * 10 / er";

            // Assert
            Assert.ThrowsException<NotCorrectExpressionException>(() => _service.CalculateManualExpression(expression));
        }

        /// <summary>
        /// Check string expression without enough  operators, should throw OperatorErrorException.
        /// </summary>
        [TestMethod]
        public void StringExpression_NotEnoughOperators_EnumNoOperators()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10   9";

            // Assert
            Assert.ThrowsException<OperatorErrorException>(() => _service.CalculateManualExpression(expression));
        }

        /// <summary>
        /// Check string expression with empty input, should throw EmptyExpressionException.
        /// </summary>
        [TestMethod]
        public void StringExpression_EmptyString_EnumNone()
        {
            // Arrange
            var expression = " ";

            // Assert
            Assert.ThrowsException<EmptyExpressionException>(() => _service.CalculateManualExpression(expression));
        }
    }
}
