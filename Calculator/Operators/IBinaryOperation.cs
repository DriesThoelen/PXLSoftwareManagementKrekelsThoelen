namespace Calculator.Operators
{
    internal interface IBinaryOperation : IOperation
    {
        IOperation OperationLeft { get; }
        IOperation OperationRight { get; set; }

        /// <summary>
        /// The <see cref="char"/> identifying this operator.
        /// </summary>
        /// <remarks>constant, immutable and unique per class.</remarks>
        char OperatorSign { get; }

        double Calculate(double left, double right);
    }
}