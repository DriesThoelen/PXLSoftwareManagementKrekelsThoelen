using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Calculator
{
    public class CalculatingUnit
    {
        //private OperationExtractor operationExtractor = new OperationExtractor();
        private AddOperator addOperator = new AddOperator();
        private SubtractOperator subtractOperator = new SubtractOperator();
        private MultiplyOperator multiplyOperator = new MultiplyOperator();
        private DivisionOperator divisionOperator = new DivisionOperator();


        public double Calculate(double[] addOperations, double[] subtractOperations, double[] multiplyOperations, double[] divisionOperations)
        {
            double result = 0.0;

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
            double intermediateResult = 0.0;

            if (operationSign.Equals(MultiplyOperator.SYMBOL))
            {
                intermediateResult += multiplyOperator.Multiply(operations);
            }

            if (operationSign.Equals(DivisionOperator.SYMBOL))
            {
                intermediateResult += divisionOperator.Divide(operations);
            }

            return intermediateResult;
        }
    }
}
