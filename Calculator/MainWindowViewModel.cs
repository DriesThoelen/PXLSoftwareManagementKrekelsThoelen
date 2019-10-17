using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
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

        string operationString = "";

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
                // Add the key to the input string.
                OperationString += key;
                operandBuffer.Append(key);
            });

            AddOperationSignCommand = new RelayCommand<char>((key) =>
            {
                ParseDoubleAndOrderByOperation(key, false);

                allOperatorSigns.Add(key);
                operandBuffer.Clear();

                // Add the key to the input string.
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

                    OperationString = calculator.Calculate(AddDoubles.ToArray(), SubtractDoubles.ToArray(), MultiplyDoubles.ToArray(), DivideDoubles.ToArray()).ToString(CultureInfo.CurrentCulture);
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
                ((RelayCommand)DeleteNumberCommand).CanExecute(true);
            }

            get => operationString;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ParseDoubleAndOrderByOperation(char key, bool finalCalculation)
        {
            switch (key)
            {
                case MultiplyOperator.SYMBOL:
                    {
                        if (operationString.Contains(AddOperator.SYMBOL) || operationString.Contains(SubtractOperator.SYMBOL))
                        {
                            if (operationString.Contains(AddOperator.SYMBOL))
                            {
                                CalculateImmediateResult(AddDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }

                            if (operationString.Contains(SubtractOperator.SYMBOL))
                            {
                                CalculateImmediateResult(SubtractDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }

                        }
                        else
                        {
                            MultiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }

                        break;
                    }
                case DivisionOperator.SYMBOL:
                    {
                        if (operationString.Contains(AddOperator.SYMBOL) || operationString.Contains(SubtractOperator.SYMBOL))
                        {
                            if (operationString.Contains(AddOperator.SYMBOL))
                            {
                                CalculateImmediateResult(AddDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }

                            if (operationString.Contains(SubtractOperator.SYMBOL))
                            {
                                CalculateImmediateResult(SubtractDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            DivideDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }

                        break;
                    }
                case AddOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                MultiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(AddDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                DivideDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(AddDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            AddDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }
                        break;
                    }
                case SubtractOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                MultiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(SubtractDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                DivideDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(SubtractDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            SubtractDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }
                        break;
                    }
            }
        }
        private void CalculateImmediateResult(ICollection<double> output, char operatorSign, bool finalCalculation, char caller)
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
                        switch (operatorSign)
                        {
                            case MultiplyOperator.SYMBOL when MultiplyDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, MultiplyDoubles.ToArray());
                                MultiplyDoubles.Clear();
                                break;
                            case DivisionOperator.SYMBOL when DivideDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, DivideDoubles.ToArray());
                                DivideDoubles.Clear();
                                break;
                        }
                    }

                    if (operationString.Contains(SubtractOperator.SYMBOL))
                    {
                        switch (operatorSign)
                        {
                            case MultiplyOperator.SYMBOL when MultiplyDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, MultiplyDoubles.ToArray());
                                MultiplyDoubles.Clear();
                                break;
                            case DivisionOperator.SYMBOL when DivideDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, DivideDoubles.ToArray());
                                DivideDoubles.Clear();
                                break;
                        }
                    }
                }
            }

            output.Add(intermediateResult);
        }
    }
}
