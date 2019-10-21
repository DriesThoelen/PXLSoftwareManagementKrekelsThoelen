using System.Globalization;

namespace Calculator.Operators
{
    internal class FixedValueOperation : IOperation
    {
        private double Digit { get; }
        public int Priority => int.MaxValue;

        public FixedValueOperation(double digit)
        {
            this.Digit = digit;
        }

        public double Calculate()
        {
            return Digit;
        }

        public override string ToString()
        {
            return Digit.ToString(CultureInfo.InvariantCulture);
        }

        public static implicit operator FixedValueOperation(double digit) => new FixedValueOperation(digit);
    }
}