namespace Calculator.Operators
{
    internal sealed class SubtractOperator : IBinaryOperator
    {
        public static readonly IBinaryOperator Singleton = new SubtractOperator();

        private SubtractOperator()
        {
        }
        
        public char Symbol => '-';

        public int Priority => 1;

        public double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}