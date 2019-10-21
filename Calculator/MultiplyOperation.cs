namespace Calculator
{
    class MultiplyOperation : DuoOperation
    {
        public MultiplyOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        protected override char OperatorSign => '*';

        public override int Priority => 2;

        protected override double Calculate(double left, double right)
        {
            return left * right;
        }
    }
}