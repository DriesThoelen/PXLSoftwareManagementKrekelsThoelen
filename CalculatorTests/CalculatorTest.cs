using System.Collections.Generic;
using Calculator;
using NUnit.Framework;
using Calculator.Operators;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculatorTest
    {
        [TestCase(1.0, 2.0)]
        [TestCase(1.1, 2.0)]
        [TestCase(1.0, 2.2)]
        [TestCase(1.1, 2.2)]
        [TestCase(1.0, 0.0)]
        [TestCase(0.0, 2.0)]
        public void ShouldAddTwoNumbersCorrectly(double arg1, double arg2)
        {
            //Arrange
            var binaryOperator = AddOperator.Singleton;
            //Act
            var result = binaryOperator.Calculate(arg1, arg2);
            //Assert
            var expected = arg1 + arg2;
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(2.0, 1.1)]
        [TestCase(2.2, 1.0)]
        [TestCase(2.2, 1.1)]
        [TestCase(0.0, 1.0)]
        [TestCase(2.0, 0.0)]
        [TestCase(3.0, 2.0)]
        [TestCase(4.0, 3.0)]
        [TestCase(5.0, 4.0)]
        public void ShouldSubtractTwoNumbersCorrectly(double arg1, double arg2)
        {
            //Arrange
            var binaryOperator = SubtractOperator.Singleton;
            //Act
            var result = binaryOperator.Calculate(arg1, arg2);
            //Assert
            var expected = arg1 - arg2;
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(2.0, 1.1)]
        [TestCase(2.2, 1.0)]
        [TestCase(2.2, 1.1)]
        [TestCase(2.0, 0.0)]
        [TestCase(0.0, 1.0)]
        [TestCase(0.0, 0.0)]
        [TestCase(3.0, 2.0)]
        [TestCase(4.0, 3.0)]
        [TestCase(5.0, 4.0)]
        public void ShouldMultiplyTwoNumbersCorrectly(double arg1, double arg2)
        {
            //Arrange
            var binaryOperator = MultiplyOperator.Singleton;
            //Act
            var result = binaryOperator.Calculate(arg1, arg2);
            //Assert
            var expected = arg1 * arg2;
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(2.0, 1.1)]
        [TestCase(2.2, 1.0)]
        [TestCase(2.2, 1.1)]
        [TestCase(0.0, 1.0)]
        [TestCase(2.0, 0.001)]
        [TestCase(3.0, 2.0)]
        [TestCase(4.0, 3.0)]
        [TestCase(5.0, 4.0)]
        public void ShouldDivideTwoNumbersCorrectly(double arg1, double arg2)
        {
            //Arrange
            var binaryOperator = DivideOperator.Singleton;
            //Act
            var result = binaryOperator.Calculate(arg1, arg2);
            //Assert
            var expected = arg1 / arg2;
            Assert.That(result, Is.EqualTo(expected));
        }


        private static IEnumerable<object> Operators()
        {
            yield return new object[] {AddOperator.Builder(), AddOperator.Singleton.Symbol};
            yield return new object[] {SubtractOperator.Builder(), SubtractOperator.Singleton.Symbol};
            yield return new object[] {MultiplyOperator.Builder(), MultiplyOperator.Singleton.Symbol};
            yield return new object[] {DivideOperator.Builder(), DivideOperator.Singleton.Symbol};
        }

        [TestCaseSource(nameof(Operators))]
        public void ShouldBuildOperation(object duoOperatorArgument, char operatorSign)
        {
            // Arrange
            var duoOperator = duoOperatorArgument as IBinaryOperationBuilder;
            Assert.NotNull(duoOperator);
            FixedValueOperation operandLeft = 0.0;
            FixedValueOperation operandRight = 1.0;

            // Act
            duoOperator.WithLeftOperand(operandLeft);
            duoOperator.WithRightOperand(operandRight);
            var operation = duoOperator.Build();
            var render = operation.ToString();

            // Assert
            Assert.IsInstanceOf(duoOperator.GetType().GetGenericArguments()[0], operation);
            Assert.AreEqual($"({operandLeft} {operatorSign} {operandRight})", render);
        }

        [TestCase('+', '+')]
        [TestCase('+', '-')]
        [TestCase('-', '+')]
        [TestCase('-', '-')]
        [TestCase('*', '+')]
        [TestCase('*', '-')]
        [TestCase('*', '*')]
        [TestCase('*', '/')]
        [TestCase('/', '+')]
        [TestCase('/', '-')]
        [TestCase('/', '*')]
        [TestCase('/', '/')]
        public void ShouldPrioritizeFirstOperator(char firstSymbol, char secondSymbol)
        {
            // Arrange
            const double operand = 1.0;

            // Act
            var render = RenderOperationTree(operand, firstSymbol, secondSymbol);

            // Assert
            Assert.AreEqual($"(({operand} {firstSymbol} {operand}) {secondSymbol} {operand})", render);
        }

        [TestCase('+', '*')]
        [TestCase('+', '/')]
        [TestCase('-', '*')]
        [TestCase('-', '/')]
        public void ShouldPrioritizeSecondOperator(char firstSymbol, char secondSymbol)
        {
            // Arrange
            const double operand = 1.0;

            // Act
            var render = RenderOperationTree(operand, firstSymbol, secondSymbol);

            // Assert
            Assert.AreEqual($"({operand} {firstSymbol} ({operand} {secondSymbol} {operand}))", render);
        }

        private string RenderOperationTree(double operand, char firstSymbol, char secondSymbol)
        {
            // Arrange part
            var operator1 = BinaryOperationBuilder.FromSymbol(firstSymbol);
            var operator2 = BinaryOperationBuilder.FromSymbol(secondSymbol);
            var operationTree = new OperationTree();

            // Act part
            operationTree.PushValue(operand);
            operationTree.PushOperator(operator1);
            operationTree.PushValue(operand);
            operationTree.PushOperator(operator2);
            operationTree.PushValue(operand);
            return operationTree.ToString();
        }
    }
}