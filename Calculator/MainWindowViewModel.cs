using System;
using System.ComponentModel;
using System.Globalization;
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
        private readonly StringBuilder operandBuffer = new StringBuilder();

        public ICommand AddNumberCommand { get; }

        public ICommand AddOperationSignCommand { get; }

        public ICommand DeleteNumberCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand CalculateCommand { get; }

        private string operationString = "";

        private OperationTree operationTree;

        private readonly Regex regEx = new Regex(@"\d+" + "[" +
                                                 "\\" + AddOperation.Symbol +
                                                 "\\" + SubtractOperation.Symbol +
                                                 "\\" + MultiplyOperation.Symbol +
                                                 "\\" + DivideOperation.Symbol +
                                                 "]+" + @"\d+");

        public MainWindowViewModel()
        {
            operationTree = new OperationTree();

            AddNumberCommand = new RelayCommand<string>((key) =>
            {
                // Add the operatorSymbol to the input string.
                OperationString += key;
                operandBuffer.Append(key);
            });

            AddOperationSignCommand = new RelayCommand<string>((key) =>
            {
                PushValue(); // todo: state pattern? (end of number input state)

                var symbol = key[0];
                // Add the operatorSymbol to the input string.
                OperationString += symbol;

                PushOperator(symbol);
            });

            DeleteNumberCommand = new RelayCommand(() =>
                {
                    // Strip a character from the input string.
                    OperationString = OperationString[..^1];
                    operandBuffer.Remove(operandBuffer.Length - 1, 1);
                },
                () => operandBuffer.Length > 0);

            ClearCommand = new RelayCommand(() =>
                {
                    // Clear the input string.
                    OperationString = string.Empty;
                    operandBuffer.Clear();
                    operationTree = new OperationTree();
                },
                () => OperationString.Length > 0);

            CalculateCommand = new RelayCommand(() =>
                {
                    PushValue();
                    OperationString = operationTree.Calculate().ToString(CultureInfo.CurrentCulture);
                },
                () => regEx.IsMatch(OperationString));
        }

        private void PushValue()
        {
            if (operandBuffer.Length == 0)
            {
                return;
            }

            var currentNumber = double.Parse(operandBuffer.ToString());
            operationTree.PushValue(currentNumber);
            operandBuffer.Clear();
        }

        private void PushOperator(char symbol)
        {
            var operationBuilder = symbol switch
            {
                '*' => (IDuoOperationBuilder<DuoOperation>) MultiplyOperation.Builder(),
                '/' => DivideOperation.Builder(),
                '+' => AddOperation.Builder(),
                '-' => SubtractOperation.Builder(),
                _ => throw new ArgumentException("Unknown operator symbol", nameof(symbol))
            };
            operationTree.PushOperator(operationBuilder);
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