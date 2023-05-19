// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace GletredEdShare.CoreModule
{
    public abstract class ViewModelBase : NotifyPropertyChangedBase, INotifyDataErrorInfo
    {
        protected Dictionary<string, IEnumerable?> Errors = new();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public bool HasErrors => Errors.Values.Any(_ => _ != null);

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return Errors.Values;
            }
            else
            {
                Errors.TryGetValue(propertyName, out var error);
                return error!;
            }
        }

        public void SetErrors(IEnumerable value, [CallerMemberName] string? propertyName = "")
        {
            if (propertyName == null)
            {
                return;
            }

            Errors[propertyName] = value;
            NotifyErrorsChanged(propertyName);
        }

        public void ClearError([CallerMemberName] string? propertyName = "")
        {
            if (propertyName == null || !Errors.ContainsKey(propertyName))
            {
                return;
            }

            Errors.Remove(propertyName);
            NotifyPropertyChanged(nameof(HasErrors));
        }

        public void ClearErrors()
        {
            this.Errors.Clear();
            NotifyPropertyChanged(nameof(HasErrors));
        }

        public void NotifyErrorsChanged([CallerMemberName] string? propertyName = "")
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
