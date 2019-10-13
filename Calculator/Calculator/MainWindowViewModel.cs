using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Calculator.Annotations;
using GalaSoft.MvvmLight.CommandWpf;

namespace Calculator
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand AddNumberCommand { protected set; get; }

        public ICommand DeleteNumberCommand { protected set; get; }

        string operationString = "";

        public MainWindowViewModel()
        {
            AddNumberCommand = new RelayCommand<string>((key) =>
            {
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
    }
}
