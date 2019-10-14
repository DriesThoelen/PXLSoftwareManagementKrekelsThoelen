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
        private OperationExtractor operationExtractor = new OperationExtractor();
        private AddOperator addOperator = new AddOperator();
        private SubstractOperator substractOperator = new SubstractOperator();
        private MultiplyOperator multiplyOperator = new MultiplyOperator();
        private DivisionOperator divisionOperator = new DivisionOperator();


        public double Calculate(string operationString)
        {
            double result = 0.0;

            List<Operation> operations = operationExtractor.Extract(operationString);

            foreach (Operation operation in operations)
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
                        result += substractOperator.Substract(new[] { operation.Operant1, operation.Operant2 });
                        break;
                    }
                }
            }

            return result;
        }
    }
}
