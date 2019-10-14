using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    public class MultiplyOperator
    {
        public double Multiply(params double[] args)
        {
            double result = args[0];

            foreach (double arg in args.Skip(1))
            {
                result *= arg;
            }

            return result;
        }
    }
}
