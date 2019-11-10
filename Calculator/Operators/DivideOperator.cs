namespace Calculator.Operators
{
    internal class DivideOperator : BinaryOperation, IBinaryOperator
    {
        public const char Symbol = '/';
        public const int DefaultPriority = 2;

        public DivideOperator(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<DivideOperator> Builder()
        {
            return new BinaryOperationBuilder<DivideOperator>(DefaultPriority,
                (leftOperand, rightOperand) => new DivideOperator(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        public override double Calculate(double left, double right)
        {
            return left / right;
        }
    }
}