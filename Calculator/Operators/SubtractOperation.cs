namespace Calculator.Operators
{
    internal class SubtractOperation : BinaryOperation
    {
        public const char Symbol = '-';
        public const int DefaultPriority = 1;

        public SubtractOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<SubtractOperation> Builder()
        {
            return new BinaryOperationBuilder<SubtractOperation>(DefaultPriority,
                (leftOperand, rightOperand) => new SubtractOperation(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}