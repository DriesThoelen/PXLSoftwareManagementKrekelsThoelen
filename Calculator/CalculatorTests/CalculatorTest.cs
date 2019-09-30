using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

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
    }
}