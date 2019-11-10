using System;

namespace Calculator.Operators
{
    internal class PlaceHolderOperation : IOperation
    {
        public int Priority => int.MinValue;
        public static readonly PlaceHolderOperation Singleton = new PlaceHolderOperation();

        private PlaceHolderOperation()
        {
        }

        public double Calculate()
        {
            throw new InvalidOperationException("A placeholder cannot be calculated");
        }

        public override string ToString() => "...";
    }
}