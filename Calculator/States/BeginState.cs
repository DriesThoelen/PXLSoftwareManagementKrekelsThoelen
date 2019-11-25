using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.States
{
    class BeginState : State
    {
        public void PushValue(StringBuilder operandBuffer, OperationTree operationTree, object value = null)
        {
            //Do Nothing
        }
    }
}
