using System;

namespace Calculator.Operators
{
    internal class BinaryOperationBuilder : IBinaryOperationBuilder
    {
        private IOperation? leftOperand;
        private IOperation? rightOperand;
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

            return new BinaryOperation(leftOperand, binaryOperator, rightOperand);
        }

        internal static IBinaryOperationBuilder FromSymbol(char symbol)
        {
            if (symbol == MultiplyOperator.Singleton.Symbol)
            {
                return MultiplyOperator.Builder();
            }

            if (symbol == DivideOperator.Singleton.Symbol)
            {
                return DivideOperator.Builder();
            }

            if (symbol == AddOperator.Singleton.Symbol)
            {
                return AddOperator.Builder();
            }

            if (symbol == SubtractOperator.Singleton.Symbol)
            {
                return SubtractOperator.Builder();
            }

            throw new ArgumentException("Unknown operator symbol", nameof(symbol));
        }
    }
}