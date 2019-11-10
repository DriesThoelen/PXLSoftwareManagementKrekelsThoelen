using System;

namespace Calculator.Operators
{
    internal abstract class BinaryOperation : IOperation
    {
        private IOperation OperationLeft { get; }
        private IOperation OperationRight { get; set; }


        public abstract char OperatorSign { get; }
        public abstract int Priority { get; }

        public T Insert<T>(IBinaryOperationBuilder<T> builder) where T : BinaryOperation
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

        protected BinaryOperation(IOperation operationLeft, IOperation operationRight)
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