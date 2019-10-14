using NUnit.Framework;
using System.Linq;

namespace CalculatorTests
{
    [TestFixture]
    public class CalculatorTest
    {
        [TestCase(1.0, 2.0)]
        [TestCase(1.0, 2.0, 3.0)]
        [TestCase(1.0, 2.0, 3.0, 4.0)]
        [TestCase(1.0, 2.0, 3.0, 4.0, 5.0)]
        public void ShouldAddTwoOrMoreNumbersCorrectly(params double[] args)
        {
            //Arrange
            Calculator.AddOperator sut = new Calculator.AddOperator();
            //Act
            double result = sut.Add(args);
            //Assert
            double expected = args.Sum();
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(3.0, 2.0, 1.0)]
        [TestCase(4.0, 3.0, 2.0, 1.0)]
        [TestCase(5.0, 4.0, 3.0, 2.0, 1.0)]
        public void ShouldSubstractTwoOrMoreNumbersCorrectly(params double[] args)
        {
            //Arrange
            Calculator.SubtractOperator sut = new Calculator.SubtractOperator();
            //Act
            double result = sut.Subtract(args);
            //Assert
            double[] substractors = args.Skip(1).ToArray();
            double[] number = { args[0] };
            while (substractors.Length > 0)
            {
                number[0] = number.Zip(substractors, (n1, n2) => n1 - n2).First();
                substractors = substractors.Skip(1).ToArray();
            }

            double expected = number[0];
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(3.0, 2.0, 1.0)]
        [TestCase(4.0, 3.0, 2.0, 1.0)]
        [TestCase(5.0, 4.0, 3.0, 2.0, 1.0)]
        public void ShouldMultiplyTwoOrMoreNumbersCorrectly(params double[] args)
        {
            //Arrange
            Calculator.MultiplyOperator sut = new Calculator.MultiplyOperator();
            //Act
            double result = sut.Multiply(args);
            //Assert
            double[] multipliers = args.Skip(1).ToArray();
            double[] number = { args[0] };
            while (multipliers.Length > 0)
            {
                number[0] = number.Zip(multipliers, (n1, n2) => n1 * n2).First();
                multipliers = multipliers.Skip(1).ToArray();
            }

            double expected = number[0];
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(2.0, 1.0)]
        [TestCase(3.0, 2.0, 1.0)]
        [TestCase(4.0, 3.0, 2.0, 1.0)]
        [TestCase(5.0, 4.0, 3.0, 2.0, 1.0)]
        public void ShouldDivideTwoOrMoreNumbersCorrectly(params double[] args)
        {
            //Arrange
            Calculator.DivisionOperator sut = new Calculator.DivisionOperator();
            //Act
            double result = sut.Divide(args);
            //Assert
            double[] dividers = args.Skip(1).ToArray();
            double[] number = { args[0] };
            while (dividers.Length > 0)
            {
                number[0] = number.Zip(dividers, (n1, n2) => n1 / n2).First();
                dividers = dividers.Skip(1).ToArray();
            }

            double expected = number[0];
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}