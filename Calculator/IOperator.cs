using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    interface IOperator
    {
        double Calculate(params double[] operands);
    }
}
