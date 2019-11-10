namespace Calculator.Operators
{
    internal sealed class AddOperator : IBinaryOperator
    {
        public static readonly IBinaryOperator Singleton = new AddOperator();

        private AddOperator()
        {
        }

        public static IBinaryOperationBuilder Builder()
        {
            return new BinaryOperationBuilder(Singleton.Priority, 
                (leftOperand, rightOperand) => new BinaryOperation(leftOperand, Singleton, rightOperand));
        }

        public char Symbol => '+';

        public int Priority => 1;

        public double Calculate(double left, double right)
        {
            return left + right;
        }
    }
}