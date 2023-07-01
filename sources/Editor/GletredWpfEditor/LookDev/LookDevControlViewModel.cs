// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.ControlModule;
using System.Collections.ObjectModel;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.LookDev
{
    public class LookDevControlViewModel : ControlViewModel
    {
        private int _mainTabSelectIndex;
        private ObservableCollection<DockingWindowViewModel> _mainTab = null!;

        public int MainTabSelectIndex
        {
            get => _mainTabSelectIndex;
            set
            {
                _mainTabSelectIndex = value;
                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<DockingWindowViewModel> MainTab
        {
            get => _mainTab;
            set
            {
                _mainTab = value;
                NotifyPropertyChanged();
            }
        }


        public LookDevControlViewModel()
        {

        }
    }
}
