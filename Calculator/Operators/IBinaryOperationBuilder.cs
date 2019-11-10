namespace Calculator.Operators
{
    internal interface IBinaryOperationBuilder
    {
        IBinaryOperationBuilder WithLeftOperand(IOperation operand);
        int Priority { get; }
        IBinaryOperation Build();
    }
}