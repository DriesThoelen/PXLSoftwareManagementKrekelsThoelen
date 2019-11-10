using System;

namespace Calculator.Operators
{
    internal static class BinaryOperators
    {
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