namespace Calculator.Operators
{
    /// <summary>
    /// IOperator represents an operator or function able to act on two or more operands.
    /// </summary>
    interface IOperator
    {
        /// <summary>
        /// Calculate the result of applying this operator to the supplied <see cref="operands"/>.
        /// </summary>
        /// <param name="operands">The values supplied to this operator</param>
        /// <returns>The result, or an exception if the given operands don't allow calculating a result</returns>
        /// <remarks>The result may be influenced by overflow, underflow, or other effects.
        /// This method does not guarantee sensible results on <i>weird</i> inputs.</remarks>
        double Calculate(params double[] operands);

        /// <summary>
        /// The <see cref="char"/> identifying this operator.
        /// </summary>
        /// <remarks>constant, immutable and unique per class.</remarks>
        char Symbol { get; }
    }
}
