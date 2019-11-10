namespace Calculator.Operators
{
    internal class AddOperator : BinaryOperation, IBinaryOperator
    {
        public const char Symbol = '+';
        public const int DefaultPriority = 1;

        public AddOperator(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        public static IBinaryOperationBuilder<AddOperator> Builder()
        {
            return new BinaryOperationBuilder<AddOperator>(DefaultPriority, 
                (leftOperand, rightOperand) => new AddOperator(leftOperand, rightOperand));
        }

        public override char OperatorSign => Symbol;

        public override int Priority => DefaultPriority;

        public override double Calculate(double left, double right)
        {
            return left + right;
        }
    }
}