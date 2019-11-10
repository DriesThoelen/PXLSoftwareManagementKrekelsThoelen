using System;
using Calculator.Operators;

namespace Calculator
{
    internal class OperationTree
    {
        /// <summary>
        /// The lightest, lowest priority operation, the root of the tree
        /// </summary>
        private IOperation rootOperation;

        /// <summary>
        /// The operation that was changed last, or is most likely to change next.
        /// </summary>
        private IOperation currentOperation;

        public OperationTree()
        {
            rootOperation = PlaceHolderOperation.Singleton;
            currentOperation = rootOperation;
        }

        internal void PushValue(double currentNumber)
        {
            FixedValueOperation fixedValue = currentNumber;
            if (currentOperation is BinaryOperation duoOperation)
            {
                duoOperation.Insert(fixedValue);
            }
            else
            {
                rootOperation = fixedValue;
                currentOperation = rootOperation;
            }
        }

        internal void PushOperator(IBinaryOperator binaryOperator)
        {
            currentOperation = binaryOperator.Priority > rootOperation.Priority
                ? InsertRight(binaryOperator)
                : InsertUp(binaryOperator);
        }

        private IOperation InsertUp(IBinaryOperator binaryOperator)
        {
            var newOperation = new BinaryOperation(rootOperation, binaryOperator);
            rootOperation = newOperation;
            return newOperation;
        }

        private IOperation InsertRight(IBinaryOperator binaryOperator)
        {
            if (rootOperation is BinaryOperation rootDuoOperation)
            {
                return rootDuoOperation.Insert(binaryOperator);
            }
            throw new InvalidOperationException("Previous operand not found for binary operator");
        }

        public double Calculate()
        {
            return rootOperation.Calculate();
        }

        public override string ToString()
        {
            return rootOperation.ToString();
        }
    }
}