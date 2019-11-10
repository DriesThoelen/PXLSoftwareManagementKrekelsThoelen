namespace Calculator.Operators
{
    internal sealed class MultiplyOperator : IBinaryOperator
    {
        public static readonly IBinaryOperator Singleton = new MultiplyOperator();

        private MultiplyOperator()
        {
        }
        
        public char Symbol => '*';

        public int Priority => 2;

        public double Calculate(double left, double right)
        {
            return left * right;
        }
    }
}