namespace Calculator.Operators
{
    internal interface IBinaryOperation : IOperation, IBinaryOperator
    {
        IOperation OperationLeft { get; }
        IOperation OperationRight { get; set; }
    }
}