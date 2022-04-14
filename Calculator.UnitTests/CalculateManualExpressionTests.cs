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
        /// Check string exepression with correct input and correct result of exeption.
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
        /// Check string expression with correct input and correct result, EnumErrors will be "success" .
        /// </summary>
        [TestMethod]
        public void StringExpression_InCorrectExpression_EnumNoOperators()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5-";
            var expected = EnumStatus.OperatorsError;

            // Act
            //var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.ThrowsException<OperatorErrorException>(_service.CalculateManualExpression(expression))
        }

        /// <summary>
        /// Check string expression with correct input and correct result, EnumErrors will be "success" .
        /// </summary>
        [TestMethod]
        public void StringExpression_CorrectExpression_EnumSuccess()
        {
            // Arrange
            var expression = "1 + 2 - 3 * 10 / 5";
            var expected = EnumStatus.Success;

            // Act
            var actual = _service.CalculateManualExpression(expression);

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
            var expected = EnumStatus.DivisionByZero;

            // Act
            var actual = _service.CalculateManualExpression(expression);

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
            var expected = EnumStatus.NotCorrectExpression;

            // Act
            var actual = _service.CalculateManualExpression(expression);

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
            var expected = EnumStatus.OperatorsError;

            // Act
            var actual = _service.CalculateManualExpression(expression);

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
            var expected = EnumStatus.None;

            // Act
            var actual = _service.CalculateManualExpression(expression);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
