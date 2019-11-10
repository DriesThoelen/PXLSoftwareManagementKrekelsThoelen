namespace Calculator.Operators
{
    internal interface IBinaryOperationBuilder
    {
        IBinaryOperationBuilder WithLeftOperand(IOperation operand);
        IBinaryOperationBuilder WithRightOperand(IOperation operand);
        int Priority { get; }
        IBinaryOperation Build();
    }
}