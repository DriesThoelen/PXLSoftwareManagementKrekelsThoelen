namespace Calculator.Operators
{
    internal class AddOperation : BinaryOperation
    {
        public const char Symbol = '+';
        public const int DefaultPriority = 1;

        public AddOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<AddOperation> Builder()
        {
            return new BinaryOperationBuilder<AddOperation>(DefaultPriority, 
                (leftOperand, rightOperand) => new AddOperation(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        protected override double Calculate(double left, double right)
        {
            return left + right;
        }
    }
}