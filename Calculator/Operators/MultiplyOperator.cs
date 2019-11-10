namespace Calculator.Operators
{
    internal class MultiplyOperator : BinaryOperation, IBinaryOperator
    {
        public const char Symbol = '*';
        public const int DefaultPriority = 2;

        public MultiplyOperator(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<MultiplyOperator> Builder()
        {
            return new BinaryOperationBuilder<MultiplyOperator>(DefaultPriority,
                (leftOperand, rightOperand) => new MultiplyOperator(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        public override double Calculate(double left, double right)
        {
            return left * right;
        }
    }
}