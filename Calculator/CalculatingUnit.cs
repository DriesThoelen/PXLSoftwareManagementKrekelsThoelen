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
        private SubtractOperator substractOperator = new SubtractOperator();
        private MultiplyOperator multiplyOperator = new MultiplyOperator();
        private DivisionOperator divisionOperator = new DivisionOperator();


        public double Calculate(double[] addOperations, double[] substractOperations, double[] multiplyOperations, double[] divisionOperations)
        {
            double result = 0.0;

            //List<Operation> operations = operationExtractor.Extract(operationString);

            /*foreach (Operation operation in operations)
            {
                switch (operation.OperatorSign)
                {
                    case '*':
                    {
                        result += multiplyOperator.Multiply(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case '/':
                    {
                        result += divisionOperator.Divide(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case '+':
                    {
                        result += addOperator.Add(new []{ operation.Operant1, operation.Operant2 });
                        break;
                    }
                    case '-':
                    {
                        result += substractOperator.Subtract(new[] { operation.Operant1, operation.Operant2 });
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

            if (substractOperations.Length > 0)
            {
                result -= substractOperator.Subtract(substractOperations);
            }

            return result;
        }

        public double IntermediateCalculate(string operationSign, double[] operations)
        {
            double intermediateResult = 0.0;

            if (operationSign.Equals("*"))
            {
                intermediateResult += multiplyOperator.Multiply(operations);
            }

            if (operationSign.Equals("/"))
            {
                intermediateResult += divisionOperator.Divide(operations);
            }

            return intermediateResult;
        }
    }
}
