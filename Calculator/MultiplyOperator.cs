using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    public class MultiplyOperator : IOperator
    {
        public const char SYMBOL = '*';

        public double Multiply(params double[] args)
        {
            double result = args[0];

            foreach (double arg in args.Skip(1))
            {
                result *= arg;
            }

            return result;
        }

        public double Calculate(params double[] operands) => Multiply(operands);
        public char Symbol { get; } = SYMBOL;
    }
}
