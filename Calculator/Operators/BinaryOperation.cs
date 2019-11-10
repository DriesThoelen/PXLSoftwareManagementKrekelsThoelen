using System;

namespace Calculator.Operators
{
    internal sealed class BinaryOperation : IBinaryOperation
    {
        private readonly IBinaryOperator binaryOperator;
        public char OperatorSign => binaryOperator.OperatorSign;
        public int Priority => binaryOperator.Priority;

        public IOperation OperationLeft { get; }
        public IOperation OperationRight { get; set; }

        public IBinaryOperation Insert(IBinaryOperationBuilder builder)
        {
            var newOperation = builder.WithLeftOperand(OperationRight).Build();
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

        internal BinaryOperation(IOperation operationLeft, IBinaryOperator binaryOperator, IOperation operationRight)
        {
            this.OperationLeft = operationLeft;
            this.binaryOperator = binaryOperator;
            this.OperationRight = operationRight;
        }

        public double Calculate()
        {
            return binaryOperator.Calculate(OperationLeft.Calculate(), OperationRight.Calculate());
        }

        public override string ToString()
        {
            return $"({OperationLeft} {OperatorSign} {OperationRight})";
        }
    }
}