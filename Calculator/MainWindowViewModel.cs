using System;
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

        private readonly List<char> allOperatorSigns = new List<char>();
        private readonly StringBuilder operandBuffer = new StringBuilder();

        private readonly CalculatingUnit calculator = new CalculatingUnit();
        public ICommand AddNumberCommand { get; }

        public ICommand AddOperationSignCommand { get; }

        public ICommand DeleteNumberCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand CalculateCommand { get; }

        private string operationString = "";

        private Operation operation;

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

            AddNumberCommand = new RelayCommand<string>((key) =>
            {
                // Add the operatorSymbol to the input string.
                OperationString += key;
                operandBuffer.Append(key);
                CreateSingleDigitOperation();
            });

            AddOperationSignCommand = new RelayCommand<string>((key) =>
            {
                var symbol = key[0];
                CreateOperation(symbol);

                allOperatorSigns.Add(symbol);
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
                    operandBuffer.Clear();

                    OperationString = calculator.Calculate(operation).ToString();
                    allOperatorSigns.Clear();
                },
                () => regEx.IsMatch(OperationString));
        }

        private void CreateOperation(char symbol)
        {
            switch (symbol)
            {
                case '*':
                case '/':
                    operation = new Operation(operation, new Operation(1), symbol, 2);
                    break;
                case '+':
                case '-':
                    operation = new Operation(operation, new Operation(0), symbol, 1);
                    break;
            }
        }

        private void CreateSingleDigitOperation()
        {
            operation = new Operation(Double.Parse(operandBuffer.ToString()));
            operandBuffer.Clear();
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

    }
}