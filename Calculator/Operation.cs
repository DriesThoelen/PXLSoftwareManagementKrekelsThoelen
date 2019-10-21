using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Operation
    {
        public Operation OperationLeft { get; private set; }
        public Operation? OperationRight { get; set; }
        public double Digit { get; private set; }
        public char OperatorSign { get; private set; }
        public int Priority { get; private set; }

        public Operation(double digit)
        {
            this.Digit = digit;
            this.Priority = 0;
            this.OperationLeft = this;
            this.OperationRight = this;
        }

        public Operation(Operation operationLeft, char operatorSign, int priority)
        {
            this.OperationLeft = operationLeft;
            this.OperatorSign = operatorSign;
            this.Priority = priority;
        }

        public override string ToString()
        {
            if (OperatorSign == 0)
            {
                return Digit.ToString(CultureInfo.InvariantCulture);
            }

            return '(' +
                   OperationLeft.ToString() +
                   OperatorSign +
                   (OperationRight == null ? " ..." : OperationRight.ToString()) +
                   ')';
        }
    }
}