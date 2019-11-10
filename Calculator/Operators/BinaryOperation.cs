using System;

namespace Calculator.Operators
{
    internal sealed class BinaryOperation : IOperation
    {
        private readonly IBinaryOperator binaryOperator;
        public int Priority => binaryOperator.Priority;

        private IOperation OperationLeft { get; }
        private IOperation OperationRight { get; set; }

        public BinaryOperation Insert(IBinaryOperator rightOperator)
        {
            var newOperation = new BinaryOperation(OperationRight, rightOperator);
            OperationRight = newOperation;
            return newOperation;
        }

        public void Insert(FixedValueOperation valueOperation)
        {
            if (OperationRight is PlaceHolderOperation)
            {
                OperationRight = valueOperation;
            }
            else
            {
                throw new InvalidOperationException("Cannot insert value operand, no place found");
            }
        }

        private BinaryOperation(IOperation operationLeft, IBinaryOperator binaryOperator, IOperation operationRight)
        {
            this.OperationLeft = operationLeft;
            this.binaryOperator = binaryOperator;
            this.OperationRight = operationRight;
        }

        internal BinaryOperation(IOperation operationLeft, IBinaryOperator binaryOperator)
            : this(operationLeft, binaryOperator, PlaceHolderOperation.Singleton)
        {
        }

        public double Calculate()
        {
            return binaryOperator.Calculate(OperationLeft.Calculate(), OperationRight.Calculate());
        }

        public override string ToString()
        {
            return $"({OperationLeft} {binaryOperator.Symbol} {OperationRight})";
        }
    }
}