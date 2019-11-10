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

        internal void PushOperator(IBinaryOperationBuilder builder)
        {
            builder.WithRightOperand(placeHolder);

            currentOperation = builder.Priority > rootOperation.Priority
                ? InsertRight(builder)
                : InsertUp(builder);
        }

        private IOperation InsertUp(IBinaryOperationBuilder builder)
        {
            var newOperation = builder.WithLeftOperand(rootOperation).Build();
            rootOperation = newOperation;
            return newOperation;
        }

        private IOperation InsertRight(IBinaryOperationBuilder builder)
        {
            if (rootOperation is BinaryOperation rootDuoOperation)
            {
                return rootDuoOperation.Insert(builder);
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