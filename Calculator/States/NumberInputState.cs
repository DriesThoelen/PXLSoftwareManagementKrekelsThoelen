using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Calculator.States
{
    public class NumberInputState : State
    {
        void State.PushValue(StringBuilder operandBuffer, OperationTree operationTree, object? value)
        {
            if (operandBuffer?.Length == 0)
            {
                return;
            }

            var currentNumber = double.Parse(operandBuffer?.ToString() ?? throw new InvalidOperationException(), CultureInfo.CurrentCulture);
            operationTree?.PushValue(currentNumber);
            operandBuffer?.Clear();
        }
    }
}
