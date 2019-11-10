namespace Calculator.Operators
{
    /// <summary>
    /// IBinaryOperator represents an operator able to act on two operands.
    /// </summary>
    internal interface IBinaryOperator
    {
        /// <summary>
        /// The <see cref="char"/> identifying this operator.
        /// </summary>
        /// <remarks>constant, immutable and unique per class.</remarks>
        char Symbol { get; }

        double Calculate(double left, double right);

        int Priority { get; }
    }
}