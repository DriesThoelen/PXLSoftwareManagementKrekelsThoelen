namespace Calculator.Operators
{
    internal class SubtractOperation : DuoOperation
    {
        public const char Symbol = '-';
        public const int DefaultPriority = 1;

        public SubtractOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IDuoOperationBuilder<SubtractOperation> Builder()
        {
            return new DuoOperationBuilder<SubtractOperation>(DefaultPriority,
                (leftOperand, rightOperand) => new SubtractOperation(leftOperand, rightOperand));
        }

        protected override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}