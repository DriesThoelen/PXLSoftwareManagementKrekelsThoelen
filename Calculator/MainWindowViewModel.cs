using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Calculator.Operators;
using GalaSoft.MvvmLight.Command;
using JetBrains.Annotations;

namespace Calculator
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IImmutableDictionary<char, List<double>> doubles;
        private List<double> AddDoubles => doubles[AddOperator.SYMBOL];
        private List<double> SubtractDoubles => doubles[SubtractOperator.SYMBOL];
        private List<double> MultiplyDoubles => doubles[MultiplyOperator.SYMBOL];
        private List<double> DivideDoubles => doubles[DivisionOperator.SYMBOL];

        private readonly List<char> allOperatorSigns = new List<char>();
        private readonly StringBuilder operandBuffer = new StringBuilder();

        private readonly CalculatingUnit calculator = new CalculatingUnit();
        public ICommand AddNumberCommand { get; }

        public ICommand AddOperationSignCommand { get; }

        public ICommand DeleteNumberCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand CalculateCommand { get; }

        private string operationString = "";

        private readonly Regex regEx = new Regex(@"\d+" + "[" +
                                                 "\\" + AddOperator.SYMBOL +
                                                 "\\" + SubtractOperator.SYMBOL +
                                                 "\\" + MultiplyOperator.SYMBOL +
                                                 "\\" + DivisionOperator.SYMBOL +
                                                 "]+" + @"\d+");

        public MainWindowViewModel()
        {
            var doublesBuilder = ImmutableDictionary.CreateBuilder<char, List<double>>();
            doublesBuilder.Add(AddOperator.SYMBOL, new List<double>());
            doublesBuilder.Add(SubtractOperator.SYMBOL, new List<double>());
            doublesBuilder.Add(MultiplyOperator.SYMBOL, new List<double>());
            doublesBuilder.Add(DivisionOperator.SYMBOL, new List<double>());
            doubles = doublesBuilder.ToImmutable();

            AddNumberCommand = new RelayCommand<char>((key) =>
            {
                // Add the operatorSymbol to the input string.
                OperationString += key;
                operandBuffer.Append(key);
            });

            AddOperationSignCommand = new RelayCommand<char>((key) =>
            {
                ParseDoubleAndOrderByOperation(key, false);

                allOperatorSigns.Add(key);
                operandBuffer.Clear();

                // Add the operatorSymbol to the input string.
                OperationString += key;
            });

            DeleteNumberCommand = new RelayCommand(() =>
                {
                    // Strip a character from the input string.
                    OperationString = OperationString[..^1];
                },
                () => OperationString.Length > 0);

            ClearCommand = new RelayCommand(() =>
                {
                    // Clear the input string.
                    OperationString = string.Empty;
                },
                () => OperationString.Length > 0);

            CalculateCommand = new RelayCommand(() =>
                {
                    ParseDoubleAndOrderByOperation(allOperatorSigns.Last(), true);
                    operandBuffer.Clear();

                    OperationString = calculator
                        .Calculate(AddDoubles.ToArray(), SubtractDoubles.ToArray(), MultiplyDoubles.ToArray(),
                            DivideDoubles.ToArray()).ToString(CultureInfo.CurrentCulture);
                    AddDoubles.Clear();
                    SubtractDoubles.Clear();
                    MultiplyDoubles.Clear();
                    DivideDoubles.Clear();
                    allOperatorSigns.Clear();
                },
                () => regEx.IsMatch(OperationString));
        }

        public string OperationString
        {
            private set
            {
                if (operationString == value)
                {
                    return;
                }

                operationString = value;
                OnPropertyChanged(nameof(OperationString));

                // Perhaps the delete button must be enabled/disabled.
                ((RelayCommand) DeleteNumberCommand).CanExecute(true);
            }

            get => operationString;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ParseDoubleAndOrderByOperation(char operatorSymbol, bool finalCalculation)
        {
            switch (operatorSymbol)
            {
                case MultiplyOperator.SYMBOL:
                case DivisionOperator.SYMBOL:
                    var calculated = false;
                    var containedOperatorSymbol = AddOperator.SYMBOL;
                    if (operationString.Contains(containedOperatorSymbol))
                    {
                        CalculateImmediateResult(doubles[containedOperatorSymbol], operatorSymbol, finalCalculation, operatorSymbol);
                        calculated = true;
                    }

                    containedOperatorSymbol = SubtractOperator.SYMBOL;
                    if (operationString.Contains(containedOperatorSymbol))
                    {
                        CalculateImmediateResult(doubles[containedOperatorSymbol], operatorSymbol, finalCalculation, operatorSymbol);
                        calculated = true;
                    }

                    if (calculated)
                    {
                        break;
                    }

                    var operand = double.Parse(operandBuffer.ToString());
                    doubles[operatorSymbol].Add(operand);
                    break;
                case AddOperator.SYMBOL:
                case SubtractOperator.SYMBOL:
                    operand = double.Parse(operandBuffer.ToString());
                    calculated = false;
                    containedOperatorSymbol = MultiplyOperator.SYMBOL;
                    if (operationString.Contains(containedOperatorSymbol))
                    {
                        doubles[containedOperatorSymbol].Add(operand);
                        CalculateImmediateResult(doubles[operatorSymbol], containedOperatorSymbol, finalCalculation, operatorSymbol);
                        calculated = true;
                    }

                    containedOperatorSymbol = DivisionOperator.SYMBOL;
                    if (operationString.Contains(containedOperatorSymbol))
                    {
                        doubles[containedOperatorSymbol].Add(operand);
                        CalculateImmediateResult(doubles[operatorSymbol], containedOperatorSymbol, finalCalculation, operatorSymbol);
                        calculated = true;
                    }

                    if (calculated)
                    {
                        break;
                    }

                    doubles[operatorSymbol].Add(operand);
                    break;
            }
        }

        private void CalculateImmediateResult(ICollection<double> output, char operatorSymbol, bool finalCalculation, char caller)
        {
            var intermediateResult = 0.0;

            if (caller.Equals(AddOperator.SYMBOL) || caller.Equals(SubtractOperator.SYMBOL))
            {
                if (operationString.Contains(MultiplyOperator.SYMBOL))
                {
                    intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, MultiplyDoubles.ToArray());
                    MultiplyDoubles.Clear();
                }

                if (operationString.Contains(DivisionOperator.SYMBOL))
                {
                    intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, DivideDoubles.ToArray());
                    DivideDoubles.Clear();
                }
            }

            if (finalCalculation)
            {
                if (caller.Equals(MultiplyOperator.SYMBOL) || caller.Equals(DivisionOperator.SYMBOL))
                {
                    if (operationString.Contains(AddOperator.SYMBOL))
                    {
                        switch (operatorSymbol)
                        {
                            case MultiplyOperator.SYMBOL when doubles[operatorSymbol].Count > 0:
                            case DivisionOperator.SYMBOL when doubles[operatorSymbol].Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(operatorSymbol, doubles[operatorSymbol].ToArray());
                                doubles[operatorSymbol].Clear();
                                break;
                        }
                    }

                    if (operationString.Contains(SubtractOperator.SYMBOL))
                    {
                        switch (operatorSymbol)
                        {
                            case MultiplyOperator.SYMBOL when doubles[operatorSymbol].Count > 0:
                            case DivisionOperator.SYMBOL when doubles[operatorSymbol].Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(operatorSymbol, doubles[operatorSymbol].ToArray());
                                doubles[operatorSymbol].Clear();
                                break;
                        }
                    }
                }
            }

            output.Add(intermediateResult);
        }
    }
}