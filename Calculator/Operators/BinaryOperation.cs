using System;

namespace Calculator.Operators
{
    internal sealed class BinaryOperation : IOperation
    {
        private readonly IBinaryOperator binaryOperator;
        public int Priority => binaryOperator.Priority;

        private readonly IOperation operationLeft;
        private IOperation operationRight;

        public BinaryOperation Insert(IBinaryOperator rightOperator)
        {
            // Example. Old is:
            //   +
            // 5   3
            // After this.Insert(multiplyOperator) is called, new must be:
            //    +
            // 5     *
            //     3   ...
            // So multiplyOperation will be new.operationRight, taking old.operationRight (i.e. 3) as its operationLeft.
            var newOperation = new BinaryOperation(operationRight, rightOperator);
            operationRight = newOperation;
            return newOperation;
        }

        public void Insert(FixedValueOperation valueOperation)
        {
            if (operationRight is PlaceHolderOperation)
            {
                operationRight = valueOperation;
            }
            else
            {
                throw new InvalidOperationException("Cannot insert value operand, no place found");
            }
        }

        private BinaryOperation(IOperation operationLeft, IBinaryOperator binaryOperator, IOperation operationRight)
        {
            this.operationLeft = operationLeft;
            this.binaryOperator = binaryOperator;
            this.operationRight = operationRight;
        }

        internal BinaryOperation(IOperation operationLeft, IBinaryOperator binaryOperator)
            : this(operationLeft, binaryOperator, PlaceHolderOperation.Singleton)
        {
        }

        public double Calculate()
        {
            return binaryOperator.Calculate(operationLeft.Calculate(), operationRight.Calculate());
        }

        public override string ToString()
        {
            return $"({operationLeft} {binaryOperator.Symbol} {operationRight})";
        }
    }
}