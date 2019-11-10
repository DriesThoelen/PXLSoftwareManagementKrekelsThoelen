namespace Calculator.Operators
{
    internal class MultiplyOperation : BinaryOperation
    {
        public const char Symbol = '*';
        public const int DefaultPriority = 2;

        public MultiplyOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<MultiplyOperation> Builder()
        {
            return new BinaryOperationBuilder<MultiplyOperation>(DefaultPriority,
                (leftOperand, rightOperand) => new MultiplyOperation(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left * right;
        }
    }
}