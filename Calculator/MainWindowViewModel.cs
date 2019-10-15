using System.Collections.Generic;
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
        private readonly List<double> addDoubles = new List<double>();
        private readonly List<double> subtractDoubles = new List<double>();
        private readonly List<double> multiplyDoubles = new List<double>();
        private readonly List<double> divideDoubles = new List<double>();
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

                    OperationString = calculator.Calculate(addDoubles.ToArray(), subtractDoubles.ToArray(), multiplyDoubles.ToArray(), divideDoubles.ToArray()).ToString(CultureInfo.CurrentCulture);
                    addDoubles.Clear();
                    subtractDoubles.Clear();
                    multiplyDoubles.Clear();
                    divideDoubles.Clear();
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
                                CalculateImmediateResult(addDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }

                            if (operationString.Contains(SubtractOperator.SYMBOL))
                            {
                                CalculateImmediateResult(subtractDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }

                        }
                        else
                        {
                            multiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }

                        break;
                    }
                case DivisionOperator.SYMBOL:
                    {
                        if (operationString.Contains(AddOperator.SYMBOL) || operationString.Contains(SubtractOperator.SYMBOL))
                        {
                            if (operationString.Contains(AddOperator.SYMBOL))
                            {
                                CalculateImmediateResult(addDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }

                            if (operationString.Contains(SubtractOperator.SYMBOL))
                            {
                                CalculateImmediateResult(subtractDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            divideDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }

                        break;
                    }
                case AddOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                multiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(addDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                divideDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(addDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            addDoubles.Add(double.Parse(operandBuffer.ToString()));
                        }
                        break;
                    }
                case SubtractOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                multiplyDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(subtractDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                divideDoubles.Add(double.Parse(operandBuffer.ToString()));
                                CalculateImmediateResult(subtractDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            subtractDoubles.Add(double.Parse(operandBuffer.ToString()));
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
                    intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                    multiplyDoubles.Clear();
                }

                if (operationString.Contains(DivisionOperator.SYMBOL))
                {
                    intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                    divideDoubles.Clear();
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
                            case MultiplyOperator.SYMBOL when multiplyDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                                multiplyDoubles.Clear();
                                break;
                            case DivisionOperator.SYMBOL when divideDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                                divideDoubles.Clear();
                                break;
                        }
                    }

                    if (operationString.Contains(SubtractOperator.SYMBOL))
                    {
                        switch (operatorSign)
                        {
                            case MultiplyOperator.SYMBOL when multiplyDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                                multiplyDoubles.Clear();
                                break;
                            case DivisionOperator.SYMBOL when divideDoubles.Count > 0:
                                intermediateResult = calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                                divideDoubles.Clear();
                                break;
                        }
                    }
                }
            }

            output.Add(intermediateResult);
        }
    }
}
