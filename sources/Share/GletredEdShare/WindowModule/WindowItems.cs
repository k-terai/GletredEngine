// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;
using GletredEdShare.CoreModule;

namespace GletredEdShare.WindowModule
{
    public enum ToolbarType
    {
        Main,
        LevelEditor
    }

    public class WindowItemViewModel : ViewModelBase
    {
        private Uri _imageUri = null!;
        private string _name = null!;

        public Uri ImageUri { get => _imageUri; set { _imageUri = value; NotifyPropertyChanged(); } }
        public string Name { get => _name; set { _name = value; NotifyPropertyChanged(); } }
    }

    public class ButtonViewModel : WindowItemViewModel
    {
        private DelegateCommand _clickCommand = null!;
        private string _toolTip = null!;
        private double _width;

        public DelegateCommand ClickCommand
        {
            get => _clickCommand; set { _clickCommand = value; NotifyPropertyChanged(); }
        }

        public double Width { get => _width; set { _width = value; NotifyPropertyChanged(); } }

        public string ToolTip { get => _toolTip; set { _toolTip = value; NotifyPropertyChanged(); } }
    }

    public class SeparatorViewModel : WindowItemViewModel
    {

    }

    public class DataGridToolViewModel : WindowItemViewModel
    {
        private DelegateCommand _clickCommand = null!;
        private string _toolTip = null!;

        public DelegateCommand ClickCommand
        {
            get => _clickCommand;
            set
            {
                _clickCommand = value; NotifyPropertyChanged();
            }
        }
        public string ToolTip { get => _toolTip; set { _toolTip = value; NotifyPropertyChanged(); } }
    }
}
