using System.Linq;

namespace Calculator.Operators
{
    public class DivisionOperator : IOperator
    {
        public const char SYMBOL = '/';

        public double Divide(params double[] args)
        {
            double result = args[0];

            foreach (double arg in args.Skip(1))
            {
                result /= arg;
            }
            
            return result;
        }

        public double Calculate(params double[] operands) => Divide(operands);
        public char Symbol { get; } = SYMBOL;
    }
}
