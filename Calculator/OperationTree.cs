using System;
using System.Collections.Generic;
using System.Text;
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

        private readonly PlaceHolderOperation placeHolder;

        public OperationTree()
        {
            placeHolder = new PlaceHolderOperation();
            rootOperation = placeHolder;
            currentOperation = rootOperation;
        }

        public void PushValue(double currentNumber)
        {
            FixedValueOperation fixedValue = currentNumber;
            if (currentOperation is DuoOperation duoOperation)
            {
                duoOperation.Insert(fixedValue);
            }
            else
            {
                rootOperation = fixedValue;
                currentOperation = rootOperation;
            }
        }

        internal void PushOperator<T>(IDuoOperationBuilder<T> builder) where T : DuoOperation
        {
            builder.WithRightOperand(placeHolder);

            currentOperation = builder.Priority > rootOperation.Priority
                ? InsertRight(builder)
                : InsertUp(builder);
        }

        private IOperation InsertUp<T>(IDuoOperationBuilder<T> builder) where T : DuoOperation
        {
            var newOperation = builder.WithLeftOperand(rootOperation).Build();
            rootOperation = newOperation;
            return newOperation;
        }

        private IOperation InsertRight<T>(IDuoOperationBuilder<T> builder) where T : DuoOperation
        {
            if (rootOperation is DuoOperation rootDuoOperation)
            {
                return rootDuoOperation.Insert(builder);
            }
            throw new InvalidOperationException("Previous operand not found for binary operator");
        }

        public double Calculate()
        {
            return rootOperation.Calculate();
        }
    }
}