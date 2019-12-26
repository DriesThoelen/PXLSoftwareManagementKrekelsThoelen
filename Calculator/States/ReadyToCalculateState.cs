using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculator.States
{
    public class ReadyToCalculateState : State
    {
        void State.PushValue(StringBuilder operandBuffer, OperationTree operationTree, object? value)
        {
            operationTree?.Calculate();
        }
    }
}
