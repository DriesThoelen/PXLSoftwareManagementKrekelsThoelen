namespace Calculator.Operators
{
    // Used because C# doesn't allow covariant generic types (out T) in classes
    internal interface IBinaryOperationBuilder<out T> where T : BinaryOperation
    {
        IBinaryOperationBuilder<T> WithLeftOperand(IOperation operand);
        IBinaryOperationBuilder<T> WithRightOperand(IOperation operand);
        int Priority { get; }
        T Build();
    }
}