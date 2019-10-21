namespace Calculator.Operators
{
    internal class MultiplyOperation : DuoOperation
    {
        public const char Symbol = '*';
        public const int DefaultPriority = 2;

        public MultiplyOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IDuoOperationBuilder<MultiplyOperation> Builder()
        {
            return new DuoOperationBuilder<MultiplyOperation>(DefaultPriority,
                (leftOperand, rightOperand) => new MultiplyOperation(leftOperand, rightOperand));
        }

        protected override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left * right;
        }
    }
}