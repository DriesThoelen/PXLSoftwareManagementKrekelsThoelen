using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    public class DivisionOperator : IOperator
    {
        public double Divide(params double[] args)
        {
            double result = args[0];

            foreach (double arg in args.Skip(1))
            {
                result /= arg;
            }

            return result;
        }

        public double Calculate(params double[] operands) => Divide(operands);
    }
}
