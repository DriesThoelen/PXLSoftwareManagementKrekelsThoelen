using System;

namespace Calculator.Operators
{
    internal class PlaceHolderOperation : IOperation
    {
        public int Priority => int.MinValue;

        public double Calculate()
        {
            throw new InvalidOperationException("A placeholder cannot be calculated");
        }
    }
}
