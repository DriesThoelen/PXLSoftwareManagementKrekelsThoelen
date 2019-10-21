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
            FixedValueOperation value1 = arg1;
            FixedValueOperation value2 = arg2;
            var sut = new AddOperation(value1, value2);
            //Act
            var result = sut.Calculate();
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
            FixedValueOperation value1 = arg1;
            FixedValueOperation value2 = arg2;
            var sut = new SubtractOperation(value1, value2);
            //Act
            var result = sut.Calculate();
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
            FixedValueOperation value1 = arg1;
            FixedValueOperation value2 = arg2;
            var sut = new MultiplyOperation(value1, value2);
            //Act
            var result = sut.Calculate();
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
            FixedValueOperation value1 = arg1;
            FixedValueOperation value2 = arg2;
            var sut = new DivideOperation(value1, value2);
            //Act
            var result = sut.Calculate();
            //Assert
            var expected = arg1 / arg2;
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}