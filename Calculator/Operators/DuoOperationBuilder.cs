using System;

namespace Calculator.Operators
{
    // Used because C# doesn't allow covariant generic types (out T) in classes
    internal interface IDuoOperationBuilder<out T> where T : DuoOperation
    {
        IDuoOperationBuilder<T> WithLeftOperand(IOperation operand);
        IDuoOperationBuilder<T> WithRightOperand(IOperation operand);
        int Priority { get; }
        T Build();
    }

    internal class DuoOperationBuilder<T> : IDuoOperationBuilder<T> where T : DuoOperation
    {
        private IOperation? leftOperand;
        private IOperation? rightOperand;
        private readonly Func<IOperation, IOperation, T> factoryFunc;
        public int Priority { get; }

        public DuoOperationBuilder(int priority, Func<IOperation, IOperation, T> factoryFunc)
        {
            this.Priority = priority;
            this.factoryFunc = factoryFunc;
        }

        public IDuoOperationBuilder<T> WithLeftOperand(IOperation operand)
        {
            this.leftOperand = operand;
            return this;
        }

        public IDuoOperationBuilder<T> WithRightOperand(IOperation operand)
        {
            this.rightOperand = operand;
            return this;
        }

        public T Build()
        {
            if (leftOperand == null)
            {
                throw new InvalidOperationException("No left operand set. Use " + nameof(WithLeftOperand) + " to set the left operand");
            }
            if (rightOperand == null)
            {
                throw new InvalidOperationException("No right operand set. Use " + nameof(WithRightOperand) + " to set the right operand");
            }
            return factoryFunc(leftOperand, rightOperand);
        }
    }
}
