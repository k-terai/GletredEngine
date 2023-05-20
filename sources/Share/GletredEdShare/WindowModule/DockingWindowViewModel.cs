// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using GletredEdShare.ControlModule;
using GletredEdShare.CoreModule;

namespace GletredEdShare.WindowModule
{
    public class DockingWindowViewModel : ViewModelBase
    {
        public enum DockingType
        {
            None,
            Anchorable,
            Document
        }

        private bool _isVisible;
        private string _title = null!;
        private string _name = null!;
        private string _contentId = null!;
        private bool _isSelected;
        private bool _isActive;
        private string _toolTip = null!;
        private bool _canClose;
        private Uri _iconSource = null!;
        private DelegateCommand _closeCommand = null!;
        private DelegateCommand _dockCommand = null!;

        public DockingType Type { get; set; }

        public bool IsVisible { get => _isVisible; set { _isVisible = value; NotifyPropertyChanged(); } }
        public string Title { get => _title; set { _title = value; NotifyPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; NotifyPropertyChanged(); } }
        public string ContentId { get => _contentId; set { _contentId = value; NotifyPropertyChanged(); } }
        public bool IsSelected { get => _isSelected; set { _isSelected = value; NotifyPropertyChanged(); } }
        public bool IsActive { get => _isActive; set { _isActive = value; NotifyPropertyChanged(); } }
        public string ToolTip { get => _toolTip; set { _toolTip = value; NotifyPropertyChanged(); } }
        public Uri IconSource { get => _iconSource; set { _iconSource = value; NotifyPropertyChanged(); } }
        public bool CanClose { get => _canClose; set { _canClose = value; NotifyPropertyChanged(); } }
        public DelegateCommand CloseCommand { get => _closeCommand; set { _closeCommand = value; NotifyPropertyChanged(); } }

        public DelegateCommand DockCommand { get => _dockCommand; set { _dockCommand = value; NotifyPropertyChanged(); } }

        public IControl<ControlViewModel> OwnerControl { get; set; } = null!;

        public DockingWindowViewModel()
        {
            CanClose = true;
            IsActive = true;
            IsVisible = true;
        }

    }
}
