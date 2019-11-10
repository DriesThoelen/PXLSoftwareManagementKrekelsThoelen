namespace Calculator.Operators
{
    internal class SubtractOperator : BinaryOperation, IBinaryOperator
    {
        public const char Symbol = '-';
        public const int DefaultPriority = 1;

        public SubtractOperator(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<SubtractOperator> Builder()
        {
            return new BinaryOperationBuilder<SubtractOperator>(DefaultPriority,
                (leftOperand, rightOperand) => new SubtractOperator(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        public override double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}