using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.Operators;

namespace Calculator
{
    public class OperationExtractor
    {
        private readonly List<Operation> operations = new List<Operation>();
        private readonly StringBuilder operandBuilder = new StringBuilder();
        private double operand1;
        private double operand2;

        public List<Operation> Extract(string operationString)
        {
            operations.Clear();

            var operationStringBuilder = new StringBuilder(operationString);
            var operationCharArray = SortOnOperatorPriority(operationString.ToCharArray());

            while (operationStringBuilder.Length > 0)
            {
                operandBuilder.Clear();

                operand1 = 0.0;
                operand2 = 0.0;

                var operatorSign = ' ';

                for (var i = 0; i < operationCharArray.Length; i++)
                {
                    if (char.IsDigit(operationCharArray[i]))
                    {
                        operandBuilder.Append(operationCharArray[i]);
                        if (operationStringBuilder.Length > 0)
                        {
                            operationStringBuilder.Remove(0, 1);
                        }
                    }
                    else
                    {
                        if (char.IsWhiteSpace(operatorSign))
                        {
                            if (operandBuilder.Length > 0)
                            {
                                operand1 = double.Parse(operandBuilder.ToString());
                            }

                            operandBuilder.Clear();
                            operatorSign = operationCharArray[i];
                            if (operationStringBuilder.Length > 0)
                            {
                                operationStringBuilder.Remove(0, 1);
                            }
                        }
                        else
                        {
                            if (operandBuilder.Length > 0)
                            {
                                operand2 = double.Parse(operandBuilder.ToString());
                            }

                            operations.Add(new Operation(operand1, operand2, operatorSign));
                            operationCharArray = operationCharArray.Skip(i).ToArray();
                            break;
                        }
                    }
                }

                if (operationStringBuilder.Length == 0)
                {
                    if (operandBuilder.Length > 0)
                    {
                        operand2 = double.Parse(operandBuilder.ToString());
                    }
                    operandBuilder.Clear();

                    operations.Add(new Operation(operand1, operand2, operatorSign));

                    break;
                }
            }

            return operations;
        }

        private char[] SortOnOperatorPriority(char[] unsortedCharArray)
        {
            char[] sortedCharArray = {};

            var operatorChar = unsortedCharArray.SkipWhile(char.IsDigit).First();
            var secondPart = unsortedCharArray.SkipWhile(char.IsDigit).ToArray();
            var operatorCharIndex = unsortedCharArray.Length - secondPart.Length;

            switch (operatorChar)
            {
                case MultiplyOperator.SYMBOL:
                case DivisionOperator.SYMBOL:
                    sortedCharArray = unsortedCharArray.Take(operatorCharIndex).Concat(secondPart).ToArray();
                    break;
                case AddOperator.SYMBOL:
                case SubtractOperator.SYMBOL:
                    if (unsortedCharArray.Contains(MultiplyOperator.SYMBOL) || unsortedCharArray.Contains(DivisionOperator.SYMBOL))
                    {
                        var firstPart = unsortedCharArray.SkipWhile(char.IsDigit).Skip(1).ToArray();
                        sortedCharArray = firstPart.Concat(unsortedCharArray.Except(firstPart).Reverse()).ToArray();
                    }
                    else
                    {
                        sortedCharArray = unsortedCharArray;
                    }
                    break;

            }

            return sortedCharArray;
        }
    }
}
