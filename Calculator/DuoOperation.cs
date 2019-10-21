using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    public abstract class DuoOperation : IOperation
    {
        private Operation OperationLeft { get; }
        private Operation OperationRight { get; }
        protected abstract char OperatorSign { get; }

        protected DuoOperation(Operation operationLeft, Operation operationRight)
        {
            this.OperationLeft = operationLeft;
            this.OperationRight = operationRight;
        }

        public double Calculate()
        {
            return Calculate(OperationLeft.Calculate(), OperationRight.Calculate());
        }

        protected abstract double Calculate(double left, double right);

        public override string ToString()
        {
            return $"({OperationLeft} {OperatorSign} {OperationRight})";
        }
    }
}