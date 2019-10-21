namespace Calculator
{
    class SubtractOperation : DuoOperation
    {
        public SubtractOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        protected override char OperatorSign => '-';

        public override int Priority => 1;

        protected override double Calculate(double left, double right)
        {
            return left - right;
        }
    }
}