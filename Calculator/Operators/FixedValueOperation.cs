using System.Globalization;

namespace Calculator.Operators
{
    internal class FixedValueOperation : IOperation
    {
        private double Value { get; }
        public int Priority => int.MaxValue;

        public FixedValueOperation(double value)
        {
            this.Value = value;
        }

        public double Calculate()
        {
            return Value;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }

        public static implicit operator FixedValueOperation(double digit) => new FixedValueOperation(digit);
    }
}