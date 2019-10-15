using Calculator.Operators;

namespace Calculator
{
    public class CalculatingUnit
    {
        //private OperationExtractor operationExtractor = new OperationExtractor();
        private readonly AddOperator addOperator = new AddOperator();
        private readonly SubtractOperator subtractOperator = new SubtractOperator();
        private readonly MultiplyOperator multiplyOperator = new MultiplyOperator();
        private readonly DivisionOperator divisionOperator = new DivisionOperator();


        public double Calculate(double[] addOperations, double[] subtractOperations, double[] multiplyOperations, double[] divisionOperations)
        {
            var result = 0.0;

            //List<Operation> operations = operationExtractor.Extract(operationString);

            /*foreach (Operation operation in operations)
            {
                switch (operation.OperatorSign)
                {
                    case MultiplyOperator.SYMBOL:
                    {
                        result += multiplyOperator.Multiply(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case DivisionOperator.SYMBOL:
                    {
                        result += divisionOperator.Divide(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case AddOperator.SYMBOL:
                    {
                        result += addOperator.Add(new [] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case SubtractOperator.SYMBOL:
                    {
                        result += subtractOperator.Subtract(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                }
            }*/

            if (multiplyOperations.Length > 0)
            {
                result += multiplyOperator.Multiply(multiplyOperations);
            }

            if (divisionOperations.Length > 0)
            {
                result += divisionOperator.Divide(divisionOperations);
            }

            if (addOperations.Length > 0)
            {
                result += addOperator.Add(addOperations);
            }

            if (subtractOperations.Length > 0)
            {
                result -= subtractOperator.Subtract(subtractOperations);
            }

            return result;
        }

        public double IntermediateCalculate(char operationSign, double[] operations)
        {
            var intermediateResult = 0.0;

            switch (operationSign)
            {
                case MultiplyOperator.SYMBOL:
                    intermediateResult += multiplyOperator.Multiply(operations);
                    break;
                case DivisionOperator.SYMBOL:
                    intermediateResult += divisionOperator.Divide(operations);
                    break;
            }

            return intermediateResult;
        }
    }
}
