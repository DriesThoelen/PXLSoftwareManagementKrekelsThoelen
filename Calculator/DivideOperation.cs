namespace Calculator
{
    class DivideOperation : DuoOperation
    {
        public DivideOperation(IOperation operationLeft, IOperation operationRight) : base(operationLeft, operationRight)
        {
        }

        protected override char OperatorSign => '/';

        protected override double Calculate(double left, double right)
        {
            return left / right;
        }
    }
}