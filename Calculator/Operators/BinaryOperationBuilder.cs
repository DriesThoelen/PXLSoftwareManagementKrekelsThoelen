using System;

namespace Calculator.Operators
{
    internal class BinaryOperationBuilder : IBinaryOperationBuilder
    {
        private IOperation? leftOperand;
        private IOperation? rightOperand;
        private readonly Func<IOperation, IOperation, IBinaryOperation> factoryFunc;
        public int Priority { get; }

        public BinaryOperationBuilder(int priority, Func<IOperation, IOperation, IBinaryOperation> factoryFunc)
        {
            this.Priority = priority;
            this.factoryFunc = factoryFunc;
        }

        public IBinaryOperationBuilder WithLeftOperand(IOperation operand)
        {
            this.leftOperand = operand;
            return this;
        }

        public IBinaryOperationBuilder WithRightOperand(IOperation operand)
        {
            this.rightOperand = operand;
            return this;
        }

        public IBinaryOperation Build()
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

        internal static IBinaryOperationBuilder FromSymbol(char symbol) =>
            symbol switch
            {
                MultiplyOperator.Symbol => (IBinaryOperationBuilder) MultiplyOperator.Builder(),
                DivideOperator.Symbol => DivideOperator.Builder(),
                AddOperator.Symbol => AddOperator.Builder(),
                SubtractOperator.Symbol => SubtractOperator.Builder(),
                _ => throw new ArgumentException("Unknown operator symbol", nameof(symbol))
            };
    }
}
