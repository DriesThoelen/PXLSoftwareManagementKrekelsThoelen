namespace Calculator.Operators
{
    internal interface IBinaryOperation : IOperation
    {
        IOperation OperationLeft { get; }
        IOperation OperationRight { get; set; }
        char OperatorSign { get; }
        int Priority { get; }
        double Calculate();
        double Calculate(double left, double right);
    }
}