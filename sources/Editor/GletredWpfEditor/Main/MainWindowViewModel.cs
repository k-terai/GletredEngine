// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.ObjectModel;
using GletredEdShare.WindowModule;


namespace GletredWpfEditor.Main
{
    public class MainWindowViewModel : WindowViewModel
    {
        private int _leftTabSelectIndex;
        private int _rightTabSelectIndex;
        private ObservableCollection<DockingWindowViewModel> _leftTab = null!;
        private ObservableCollection<DockingWindowViewModel> _rightTab = null!;


        public int LeftTabSelectIndex
        {
            get => _leftTabSelectIndex;
            set
            {
                _leftTabSelectIndex = value;
                NotifyPropertyChanged();
            }
        }

        public int RightTabSelectIndex
        {
            get => _rightTabSelectIndex;
            set
            {
                _rightTabSelectIndex = value;
                NotifyPropertyChanged();
            }
        }


        public ObservableCollection<DockingWindowViewModel> LeftTab
        {
            get => _leftTab;
            set
            {
                _leftTab = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<DockingWindowViewModel> RightTab
        {
            get => _rightTab;
            set
            {
                _rightTab = value;
                NotifyPropertyChanged();
            }
        }


        public MainWindowViewModel()
        {
            ResourceService.Current.ChangeCulture("en-US");

            EnableToolBar = true;
            //IconUri = new Uri(Resources.icon_app, UriKind.RelativeOrAbsolute);
            MinimizeImageUri = ResourceService.Current.GetFluentIconUri(Resources.Icon_Minimize);
            MaximizeImageUri = ResourceService.Current.GetFluentIconUri(Resources.Icon_Maximize);
            RestoreImageUri = ResourceService.Current.GetFluentIconUri(Resources.Icon_Restore);
            CloseImageUri = ResourceService.Current.GetFluentIconUri(Resources.Icon_Close);
            Title = "None";
            InitCommands();
            InitLeftTab();
            InitRightTab();
        }


        private void InitCommands()
        {

        }

        private void InitLeftTab()
        {
            LeftTab = new ObservableCollection<DockingWindowViewModel>
            {
                new()
                {
                    Name = Resources.Viewport,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_Viewport),
                    OwnerControl = EditorManager.CreateViewportControl()
                },
                new()
                {
                    Name = Resources.AssetBrowser,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_AssetBrowser),
                    OwnerControl = EditorManager.CreateAssetBrowserControl()
                }
            };
        }

        private void InitRightTab()
        {
            RightTab = new ObservableCollection<DockingWindowViewModel>
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
