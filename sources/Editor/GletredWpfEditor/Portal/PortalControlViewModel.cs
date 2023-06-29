// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.ObjectModel;
using GletredEdShare.ControlModule;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.Portal
{
    public class PortalControlViewModel : ControlViewModel
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

        public PortalControlViewModel()
        {
            InitMainTab();
        }

        private void InitMainTab()
        {
            MainTab = new ObservableCollection<DockingWindowViewModel>
            {
                new()
                {
                    Name = Resources.LogViewer,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_LogViewer),
                    OwnerControl = EditorManager.CreateLogViewerControl()
                }
            };
        }
    }
}
