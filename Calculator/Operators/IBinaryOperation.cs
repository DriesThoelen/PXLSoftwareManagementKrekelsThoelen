namespace Calculator.Operators
{
    internal interface IBinaryOperation : IOperation
    {
        IOperation OperationLeft { get; }
        IOperation OperationRight { get; set; }
    }
}