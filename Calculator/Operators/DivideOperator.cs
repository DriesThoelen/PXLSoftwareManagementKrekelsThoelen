namespace Calculator.Operators
{
    internal sealed class DivideOperator : IBinaryOperator
    {
        public static readonly IBinaryOperator Singleton = new DivideOperator();

        private DivideOperator()
        {
        }

        public char Symbol => '/';

        public int Priority => 2;

        public double Calculate(double left, double right)
        {
            return left / right;
        }
    }
}