using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator
{
    abstract class DuoOperation : IOperation
    {
        private IOperation OperationLeft { get; }
        private IOperation OperationRight { get; }
        protected abstract char OperatorSign { get; }
        public abstract int Priority { get; }

        protected DuoOperation(IOperation operationLeft, IOperation operationRight)
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