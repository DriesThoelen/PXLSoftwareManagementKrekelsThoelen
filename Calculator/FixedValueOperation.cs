using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculator
{
    class FixedValueOperation : IOperation
    {
        public double Digit { get; }

        public FixedValueOperation(double digit)
        {
            this.Digit = digit;
        }

        public override string ToString()
        {
            return Digit.ToString(CultureInfo.InvariantCulture);
        }

        public double Calculate()
        {
            return Digit;
        }
    }
}
