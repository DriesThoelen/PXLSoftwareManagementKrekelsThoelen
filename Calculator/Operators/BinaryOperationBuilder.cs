using System;

namespace Calculator.Operators
{
    internal class BinaryOperationBuilder<T> : IBinaryOperationBuilder<T> where T : BinaryOperation
    {
        private IOperation? leftOperand;
        private IOperation? rightOperand;
        private readonly Func<IOperation, IOperation, T> factoryFunc;
        public int Priority { get; }

        public BinaryOperationBuilder(int priority, Func<IOperation, IOperation, T> factoryFunc)
        {
            this.Priority = priority;
            this.factoryFunc = factoryFunc;
        }

        public IBinaryOperationBuilder<T> WithLeftOperand(IOperation operand)
        {
            this.leftOperand = operand;
            return this;
        }

        public IBinaryOperationBuilder<T> WithRightOperand(IOperation operand)
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

        internal static IBinaryOperationBuilder<BinaryOperation> FromSymbol(char symbol) =>
            symbol switch
            {
                MultiplyOperation.Symbol => (IBinaryOperationBuilder<BinaryOperation>) MultiplyOperation.Builder(),
                DivideOperation.Symbol => DivideOperation.Builder(),
                AddOperation.Symbol => AddOperation.Builder(),
                SubtractOperation.Symbol => SubtractOperation.Builder(),
                _ => throw new ArgumentException("Unknown operator symbol", nameof(symbol))
            };
    }
}
