using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Calculator.Operators;
using Calculator.States;
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

        public ICommand ClearEntryCommand { get; }

        public ICommand ClearAllCommand { get; }

        public ICommand CalculateCommand { get; }

        private string operationString = "";

        private OperationTree operationTree;

        private State state;

        public MainWindowViewModel()
        {
            state = new NumberInputState();
            operationTree = new OperationTree();

            AddNumberCommand = new RelayCommand<string>((key) =>
            {
                state = new NumberInputState();
                // Add the operatorSymbol to the input string.
                OperationString += key;
                operandBuffer.Append(key);
            });

            AddOperationSignCommand = new RelayCommand<string>((key) =>
            {
                state.PushValue(operandBuffer, operationTree);

                var symbol = key[0];
                // Add the operatorSymbol to the input string.
                OperationString += symbol;

                state = new OperantInputState();

                state.PushValue(operandBuffer, operationTree, symbol);
            });

            DeleteNumberCommand = new RelayCommand(() =>
                {
                    // Strip a character from the input string.
                    OperationString = OperationString[..^1];
                    operandBuffer.Remove(operandBuffer.Length - 1, 1);
                },
                () => operandBuffer.Length > 0);

            ClearEntryCommand = new RelayCommand(() =>
                {
                    OperationString = OperationString[..^operandBuffer.Length];
                    operandBuffer.Clear();
                },
                () => operandBuffer.Length > 0);

            ClearAllCommand = new RelayCommand(() =>
                {
                    // Clear the input string.
                    OperationString = string.Empty;
                    operandBuffer.Clear();
                    operationTree = new OperationTree();
                },
                () => OperationString.Length > 0);

            CalculateCommand = new RelayCommand(() =>
                {
                    state.PushValue(operandBuffer, operationTree);
                    state = new ReadyToCalculateState();
                    state.PushValue(operandBuffer, operationTree);
                    OperationString = operationTree.Calculate().ToString(CultureInfo.CurrentCulture);
                },
                () => state is NumberInputState);
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