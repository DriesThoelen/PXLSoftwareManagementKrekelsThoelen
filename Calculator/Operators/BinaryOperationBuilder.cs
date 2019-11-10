using System;

namespace Calculator.Operators
{
    internal class BinaryOperationBuilder : IBinaryOperationBuilder
    {
        private IOperation? leftOperand;
        private readonly IBinaryOperator binaryOperator;

        public int Priority => binaryOperator.Priority;

        public BinaryOperationBuilder(IBinaryOperator binaryOperator)
        {
            this.binaryOperator = binaryOperator;
        }

        public IBinaryOperationBuilder WithLeftOperand(IOperation operand)
        {
            this.leftOperand = operand;
            return this;
        }

        public IBinaryOperation Build()
        {
            if (leftOperand == null)
            {
                throw new InvalidOperationException("No left operand set. Use " + nameof(WithLeftOperand) + " to set the left operand");
            }

            return new BinaryOperation(leftOperand, binaryOperator);
        }

        internal static IBinaryOperator FromSymbol(char symbol)
        {
            if (symbol == MultiplyOperator.Singleton.Symbol)
            {
                return MultiplyOperator.Singleton;
            }

            if (symbol == DivideOperator.Singleton.Symbol)
            {
                return DivideOperator.Singleton;
            }

            if (symbol == AddOperator.Singleton.Symbol)
            {
                return AddOperator.Singleton;
            }

            if (symbol == SubtractOperator.Singleton.Symbol)
            {
                return SubtractOperator.Singleton;
            }

            throw new ArgumentException("Unknown operator symbol", nameof(symbol));
        }
    }
}