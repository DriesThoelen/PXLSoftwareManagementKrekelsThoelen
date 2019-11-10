namespace Calculator.Operators
{
    internal class DivideOperator : IBinaryOperator
    {
        public const char Symbol = '/';
        public const int DefaultPriority = 2;
        public static readonly IBinaryOperator Singleton = new DivideOperator();

        private DivideOperator()
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
            return left / right;
        }
    }
}