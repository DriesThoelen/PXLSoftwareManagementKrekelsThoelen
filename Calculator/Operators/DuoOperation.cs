using System;

namespace Calculator.Operators
{
    internal abstract class DuoOperation : IOperation
    {
        private IOperation OperationLeft { get; }
        private IOperation OperationRight { get; set; }

        /// <summary>
        /// The <see cref="char"/> identifying this operator.
        /// </summary>
        /// <remarks>constant, immutable and unique per class.</remarks>
        protected abstract char OperatorSign { get; }
        public abstract int Priority { get; }

        public T Insert<T>(IDuoOperationBuilder<T> builder) where T : DuoOperation
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