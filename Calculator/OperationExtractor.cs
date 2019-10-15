using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.Operators;

namespace Calculator
{
    public class OperationExtractor
    {
        private List<Operation> operations = new List<Operation>();
        private StringBuilder operantBuilder = new StringBuilder();
        private StringBuilder operationStringBuilder;
        private double operant1;
        private double operant2;
        public List<Operation> Extract(string operationString)
        {
            operations.Clear();

            operationStringBuilder = new StringBuilder(operationString);
            char[] operationCharArray = SortOnOperatorPriority(operationString.ToCharArray());

            while (operationStringBuilder.Length > 0)
            {
                operantBuilder.Clear();

                operant1 = 0.0;
                operant2 = 0.0;

                char operatorSign = ' ';

                for (int i = 0; i < operationCharArray.Length; i++)
                {
                    if (Char.IsDigit(operationCharArray[i]))
                    {
                        operantBuilder.Append(operationCharArray[i]);
                        if (operationStringBuilder.Length > 0)
                        {
                            operationStringBuilder.Remove(0, 1);
                        }
                    }
                    else
                    {
                        if (char.IsWhiteSpace(operatorSign))
                        {
                            if (operantBuilder.Length > 0)
                            {
                                operant1 = Double.Parse(operantBuilder.ToString());
                            }

                            operantBuilder.Clear();
                            operatorSign = operationCharArray[i];
                            if (operationStringBuilder.Length > 0)
                            {
                                operationStringBuilder.Remove(0, 1);
                            }
                        }
                        else
                        {
                            if (operantBuilder.Length > 0)
                            {
                                operant2 = Double.Parse(operantBuilder.ToString());
                            }

                            operations.Add(new Operation(operant1, operant2, operatorSign));
                            operationCharArray = operationCharArray.Skip(i).ToArray();
                            break;
                        }
                    }
                }

                if (operationStringBuilder.Length == 0)
                {
                    if (operantBuilder.Length > 0)
                    {
                        operant2 = Double.Parse(operantBuilder.ToString());
                    }
                    operantBuilder.Clear();

                    operations.Add(new Operation(operant1, operant2, operatorSign));

                    break;
                }
            }

            return operations;
        }

        private char[] SortOnOperatorPriority(char[] unsortedCharArray)
        {
            char[] sortedCharArray = {};

            char operatorChar = unsortedCharArray.SkipWhile(c => char.IsDigit(c)).First();
            char[] secondPart = unsortedCharArray.SkipWhile(c => char.IsDigit(c)).ToArray();
            int operatorCharIndex = unsortedCharArray.Length - secondPart.Length;

            switch (operatorChar)
            {
                case MultiplyOperator.SYMBOL:
                case DivisionOperator.SYMBOL:
                    sortedCharArray = (unsortedCharArray.Take(operatorCharIndex)).Concat(secondPart).ToArray();
                    break;
                case AddOperator.SYMBOL:
                case SubtractOperator.SYMBOL:
                    if (unsortedCharArray.Contains(MultiplyOperator.SYMBOL) || unsortedCharArray.Contains(DivisionOperator.SYMBOL))
                    {
                        char[] firstPart = unsortedCharArray.SkipWhile(c => char.IsDigit(c)).Skip(1).ToArray();
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
