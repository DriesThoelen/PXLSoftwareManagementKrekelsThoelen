namespace Calculator.Operators
{
    internal class AddOperation : DuoOperation
    {
        public const char Symbol = '+';
        public const int DefaultPriority = 1;

        public AddOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IDuoOperationBuilder<AddOperation> Builder()
        {
            return new DuoOperationBuilder<AddOperation>(DefaultPriority, 
                (leftOperand, rightOperand) => new AddOperation(leftOperand, rightOperand));
        }

        protected override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left + right;
        }
    }
}