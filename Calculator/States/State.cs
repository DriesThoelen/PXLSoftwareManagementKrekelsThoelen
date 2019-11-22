using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.States
{
    interface State
    {
        void PushValue(StringBuilder operandBuffer, OperationTree operationTree, object? value = null);
    }
}
