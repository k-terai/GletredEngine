// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.


using System;
using System.Diagnostics;
using System.Windows.Input;

namespace GletredEdShare.CoreModule
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?> _canExecute;

#if WPF
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
#else
        public event EventHandler? CanExecuteChanged
        {
            add { }
            remove { }
        }
#endif

        public DelegateCommand(Action<object?> execute) : this(execute, (_) => true)
        {

        }

        public DelegateCommand(Action<object?> execute, Predicate<object?> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        public bool SafeExecute(in object canExecuteParameter, in object executeParameter)
        {
            if (!CanExecute(canExecuteParameter))
            {
                return false;
            }

            Execute(executeParameter);
            return true;
        }

        public void RaiseCanExecuteChanged(object parameter)
        {
            CanExecute(parameter);
        }
    }
}