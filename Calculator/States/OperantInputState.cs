using System;
using System.Collections.Generic;
using System.Text;
using Calculator.Operators;

namespace Calculator.States
{
    public class OperantInputState : State
    {
        void State.PushValue(StringBuilder operandBuffer, OperationTree operationTree, object? value = null)
        {
            if (value != null)
            {
                if (value is char symbol)
                {
                    operationTree.PushOperator(BinaryOperators.FromSymbol(symbol));
                }
            }
        }
    }
}
