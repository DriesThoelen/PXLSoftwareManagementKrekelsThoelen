using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
            char[] operationCharArray = operationString.ToCharArray();

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
    }
}
