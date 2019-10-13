using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class SubstractOperator
    {
        public double Substract(params double[] args)
        {
            double result = args[0];

            foreach (double arg in args.Skip(1))
            {
                result -= arg;
            }

            return result;
        }
    }
}
