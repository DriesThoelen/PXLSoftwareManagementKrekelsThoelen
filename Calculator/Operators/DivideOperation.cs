namespace Calculator.Operators
{
    internal class DivideOperation : BinaryOperation
    {
        public const char Symbol = '/';
        public const int DefaultPriority = 2;

        public DivideOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<DivideOperation> Builder()
        {
            return new BinaryOperationBuilder<DivideOperation>(DefaultPriority,
                (leftOperand, rightOperand) => new DivideOperation(leftOperand, rightOperand));
        }

        protected override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left / right;
        }
    }
}