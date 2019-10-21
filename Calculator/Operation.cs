using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Operation
    {
        public Operation OperationLeft { get; set; }
        public Operation OperationRight { get; set; }
        public double Digit { get; set; }
        public char OperatorSign { get; protected set; }
        public int Priority { get; protected set; }
        public Operation(double digit)
        {
            this.Digit = digit;
            this.Priority = 0;
        }

        public Operation(Operation operationLeft, Operation operationRight, char operatorSign, int priority)
        {
            this.OperationLeft = operationLeft;
            this.OperationRight = operationRight;
            this.OperatorSign = operatorSign;
            this.Priority = priority;
        }
    }
}
