using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private List<Double> addDoubles = new List<Double>();
        private List<Double> subtractDoubles = new List<Double>();
        private List<Double> multiplyDoubles = new List<Double>();
        private List<Double> divideDoubles = new List<Double>();
        private List<char> allOperatorSigns = new List<char>();
        private StringBuilder operantBuffer = new StringBuilder();

        private CalculatingUnit _calculator = new CalculatingUnit();
        public ICommand AddNumberCommand { protected set; get; }

        public ICommand AddOperationSignCommand { protected set; get; }

        public ICommand DeleteNumberCommand { protected set; get; }

        public ICommand ClearCommand { protected set; get; }

        public ICommand CalculateCommand { protected set; get; }

        string operationString = "";

        private Regex _regEx = new Regex(@"\d+" + "[" + 
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
                operantBuffer.Append(key);
            });

            AddOperationSignCommand = new RelayCommand<char>((key) =>
            {
                ParseDoubleAndOrderByOperation(key, false);

                allOperatorSigns.Add(key);
                operantBuffer.Clear();

                // Add the key to the input string.
                OperationString += key;
            });

            DeleteNumberCommand = new RelayCommand(() =>
                {
                    // Strip a character from the input string.
                    OperationString = OperationString.Substring(0, OperationString.Length - 1);
                },
                () =>
                {
                    // Return true if there's something to delete.
                    return OperationString.Length > 0;
                });

            ClearCommand = new RelayCommand(() =>
                {
                    // Clear the input string.
                    OperationString = String.Empty;
                },
                () =>
                {
                    // Return true if there's something to clear.
                    return OperationString.Length > 0;
                });

            CalculateCommand = new RelayCommand(() =>
                {
                    ParseDoubleAndOrderByOperation(allOperatorSigns.Last(), true);
                    operantBuffer.Clear();

                    OperationString = _calculator.Calculate(addDoubles.ToArray(), subtractDoubles.ToArray(), multiplyDoubles.ToArray(), divideDoubles.ToArray()).ToString();
                    addDoubles.Clear();
                    subtractDoubles.Clear();
                    multiplyDoubles.Clear();
                    divideDoubles.Clear();
                    allOperatorSigns.Clear();
                },
                () => { return _regEx.IsMatch(OperationString); });
        }

        public string OperationString
        {
            protected set
            {
                if (operationString != value)
                {
                    operationString = value;
                    OnPropertyChanged(nameof(OperationString));

                    // Perhaps the delete button must be enabled/disabled.
                    ((RelayCommand)DeleteNumberCommand).CanExecute(true);
                }
            }

            get { return operationString; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
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
                            multiplyDoubles.Add(Double.Parse(operantBuffer.ToString()));
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
                            divideDoubles.Add(Double.Parse(operantBuffer.ToString()));
                        }

                        break;
                    }
                case AddOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                multiplyDoubles.Add(Double.Parse(operantBuffer.ToString()));
                                CalculateImmediateResult(addDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                divideDoubles.Add(Double.Parse(operantBuffer.ToString()));
                                CalculateImmediateResult(addDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            addDoubles.Add(Double.Parse(operantBuffer.ToString()));
                        }
                        break;
                    }
                case SubtractOperator.SYMBOL:
                    {
                        if (operationString.Contains(MultiplyOperator.SYMBOL) || operationString.Contains(DivisionOperator.SYMBOL))
                        {
                            if (operationString.Contains(MultiplyOperator.SYMBOL))
                            {
                                multiplyDoubles.Add(Double.Parse(operantBuffer.ToString()));
                                CalculateImmediateResult(subtractDoubles, MultiplyOperator.SYMBOL, finalCalculation, key);
                            }
                            if (operationString.Contains(DivisionOperator.SYMBOL))
                            {
                                divideDoubles.Add(Double.Parse(operantBuffer.ToString()));
                                CalculateImmediateResult(subtractDoubles, DivisionOperator.SYMBOL, finalCalculation, key);
                            }
                        }
                        else
                        {
                            subtractDoubles.Add(Double.Parse(operantBuffer.ToString()));
                        }
                        break;
                    }
            }
        }
        private void CalculateImmediateResult(List<double> output, char operatorSign, bool finalCalculation, char caller)
        {
            double intermediateResult = 0.0;

            if (caller.Equals(AddOperator.SYMBOL) || caller.Equals(SubtractOperator.SYMBOL))
            {
                if (operationString.Contains(MultiplyOperator.SYMBOL))
                {
                    intermediateResult = _calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                    multiplyDoubles.Clear();
                }

                if (operationString.Contains(DivisionOperator.SYMBOL))
                {
                    intermediateResult = _calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                    divideDoubles.Clear();
                }
            }

            if (finalCalculation)
            {
                if (caller.Equals(MultiplyOperator.SYMBOL) || caller.Equals(DivisionOperator.SYMBOL))
                {
                    if (operationString.Contains(AddOperator.SYMBOL))
                    {
                        if (operatorSign.Equals(MultiplyOperator.SYMBOL) && multiplyDoubles.Count > 0)
                        {
                            intermediateResult = _calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                            multiplyDoubles.Clear();
                        }

                        if (operatorSign.Equals(DivisionOperator.SYMBOL) && divideDoubles.Count > 0)
                        {
                            intermediateResult = _calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                            divideDoubles.Clear();
                        }
                    }

                    if (operationString.Contains(SubtractOperator.SYMBOL))
                    {
                        if (operatorSign.Equals(MultiplyOperator.SYMBOL) && multiplyDoubles.Count > 0)
                        {
                            intermediateResult = _calculator.IntermediateCalculate(MultiplyOperator.SYMBOL, multiplyDoubles.ToArray());
                            multiplyDoubles.Clear();
                        }

                        if (operatorSign.Equals(DivisionOperator.SYMBOL) && divideDoubles.Count > 0)
                        {
                            intermediateResult = _calculator.IntermediateCalculate(DivisionOperator.SYMBOL, divideDoubles.ToArray());
                            divideDoubles.Clear();
                        }
                    }
                }
            }

            output.Add(intermediateResult);
        }
    }
}
