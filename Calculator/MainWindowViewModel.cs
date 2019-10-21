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

        private readonly CalculatingUnit calculator = new CalculatingUnit();
        public ICommand AddNumberCommand { get; }

        public ICommand AddOperationSignCommand { get; }

        public ICommand DeleteNumberCommand { get; }

        public ICommand ClearCommand { get; }

        public ICommand CalculateCommand { get; }

        private string operationString = "";

        private Operation? operationTree;

        private readonly Regex regEx = new Regex(@"\d+" + "[" +
                                                 "\\" + AddOperator.SYMBOL +
                                                 "\\" + SubtractOperator.SYMBOL +
                                                 "\\" + MultiplyOperator.SYMBOL +
                                                 "\\" + DivisionOperator.SYMBOL +
                                                 "]+" + @"\d+");

        public MainWindowViewModel()
        {
            AddNumberCommand = new RelayCommand<string>((key) =>
            {
                // Add the operatorSymbol to the input string.
                OperationString += key;
                operandBuffer.Append(key);
                operationTree = CreateSingleDigitOperation();
            });

            AddOperationSignCommand = new RelayCommand<string>((key) =>
            {
                var symbol = key[0];
                operationTree = CreateOperation(symbol);

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
                    if (operationTree == null)
                    {
                        return;
                    }

                    OperationString = calculator.Calculate(operationTree).ToString(CultureInfo.CurrentCulture);
                },
                () => regEx.IsMatch(OperationString));
        }

        private Operation? CreateOperation(char symbol)
        {
            if (operationTree?.OperationRight == null)
            {
                return operationTree;
            }
            switch (symbol)
            {
                case '*':
                case '/':
                    if (operationTree.Priority == 1)
                    {
                        operationTree.OperationRight = new Operation(operationTree.OperationRight, symbol, 2);
                        return operationTree;
                    }
                    else
                    {
                        return new Operation(operationTree, symbol, 2);
                    }
                case '+':
                case '-':
                    return new Operation(operationTree, symbol, 1);
                default:
                    return operationTree;
            }
        }

        private Operation CreateSingleDigitOperation()
        {
            var currentNumber = double.Parse(operandBuffer.ToString());
            var fixedOperation = new Operation(currentNumber);
            if (operationTree == null || operationTree.Priority == 0)
            {
                return fixedOperation;
            }

            var currentOperation = operationTree;
            var rightSubOperation = operationTree.OperationRight;
            while (rightSubOperation != null && rightSubOperation.Priority != 0)
            {
                currentOperation = rightSubOperation;
                rightSubOperation = currentOperation.OperationRight;
            }

            currentOperation.OperationRight = fixedOperation;
            operandBuffer.Clear();
            return operationTree;
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