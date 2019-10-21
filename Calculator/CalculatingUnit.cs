using Calculator.Operators;

namespace Calculator
{
    public class CalculatingUnit
    {
        public double Calculate(Operation operation)
        {
            if (operation.Digit != 0.0)
            {
                return operation.Digit;
            }

            double left = Calculate(operation.OperationLeft);
            double right = Calculate(operation.OperationRight);

            switch (operation.OperatorSign)
            {
                case '+':
                    return left + right;
                case '-':
                    return left - right;
                case '*':
                    return left * right;
                case '/':
                    return left / right;
            }

            return 0.0;
        }
    }
}
