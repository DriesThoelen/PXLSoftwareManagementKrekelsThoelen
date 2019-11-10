namespace Calculator.Operators
{
    internal class AddOperator : IBinaryOperator
    {
        public const char Symbol = '+';
        public const int DefaultPriority = 1;
        public static readonly IBinaryOperator Singleton = new AddOperator();

        private AddOperator()
        {
        }

        public static IBinaryOperationBuilder Builder()
        {
            return new BinaryOperationBuilder(DefaultPriority, 
                (leftOperand, rightOperand) => new BinaryOperation(leftOperand, Singleton, rightOperand));
        }

        public char OperatorSign => Symbol;

        public int Priority => DefaultPriority;

        public double Calculate(double left, double right)
        {
            return left + right;
        }
    }
}