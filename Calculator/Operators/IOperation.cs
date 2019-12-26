namespace Calculator.Operators
{

    /// <remarks>SE: this interface will allow us to use Liskov Substitution</remarks>
    internal interface IOperation
    {
        int Priority { get; }

        /// <summary>
        /// Calculate the result of applying this operator.
        /// </summary>
        /// <returns>The resulting value</returns>
        /// <exception cref="System.InvalidOperationException">If this operation cannot be calculated</exception>
        /// <exception cref="System.ArithmeticException">If calculating this operation leads to arithmetic, casting or conversion errors</exception>
        /// <remarks>The result may be influenced by overflow, underflow, or other effects.
        /// This method does not guarantee sensible results on <i>weird</i> inputs.</remarks>
        double Calculate();

        /// <remarks>Defined to inform the compiler this never returns null, using c# 8's null reference handling</remarks>
        string ToString();
    }
}
