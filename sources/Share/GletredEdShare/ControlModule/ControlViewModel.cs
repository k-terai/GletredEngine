// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.CoreModule;

namespace GletredEdShare.ControlModule
{
    public class ControlViewModel : ViewModelBase
    {
        private string _title = null!;
        private bool _enableToolBar;

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }

        public bool EnableToolBar
        {
            get => _enableToolBar;
            set
            {
                _enableToolBar = value;
                NotifyPropertyChanged();
            }
        }

        public ControlViewModel()
        {
            Title = string.Empty;
            EnableToolBar = true;
        }
    }
}
