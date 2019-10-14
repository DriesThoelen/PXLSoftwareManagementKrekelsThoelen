using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Operation
    {
        public double Operant1 { get; protected set; }
        public double Operant2 { get; protected set; }
        public char OperatorSign { get; protected set; }

        public Operation(double operant1, double operant2, char operatorSign)
        {
            this.Operant1 = operant1;
            this.Operant2 = operant2;
            this.OperatorSign = operatorSign;
        }
    }
}
