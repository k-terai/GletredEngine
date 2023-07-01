// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using GletredEdShare.ControlModule;
using GletredEdShare.CoreModule;
using GletredEdShare.WindowModule;
using GletredWpfEditor.LookDev;
using GletredWpfEditor.Portal;


namespace GletredWpfEditor.Main
{
    public class MainWindowViewModel : WindowViewModel
    {
        private int _mainTabSelectIndex;
        private ObservableCollection<DockingWindowViewModel> _mainTab = new();

        public DelegateCommand OpenPortalCommand { get; private set; } = null!;
        public DelegateCommand OpenLookDevCommand { get; private set; } = null!;


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

            Debug.Assert(OpenPortalCommand != null, nameof(OpenPortalCommand) + " != null");
            OpenPortalCommand.Execute(null);
        }


        private void InitCommands()
        {
            OpenPortalCommand = new DelegateCommand((_) =>
            {
                var index = GetControlIndex<PortalControlViewModel>();
                if (index != -1)
                {
                    MainTabSelectIndex = index;
                    return;
                }

                MainTab.Add(new DockingWindowViewModel
                {
                    Name = Resources.Portal,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_Portal),
                    OwnerControl = EditorManager.CreatePortalControl()
                });

                MainTabSelectIndex = MainTab.Count - 1; //Select create tab
            });

            OpenLookDevCommand = new DelegateCommand((_) =>
            {
                var index = GetControlIndex<LookDevControlViewModel>();
                if (index != -1)
                {
                    MainTabSelectIndex = index;
                    return;
                }

                MainTab.Add(new DockingWindowViewModel
                {
                    Name = Resources.LookDev,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_LookDev),
                    OwnerControl = EditorManager.CreateLookDevControl()
                });

                MainTabSelectIndex = MainTab.Count - 1; //Select create tab
            });
        }

        private int GetControlIndex<T>()
        where T : ControlViewModel
        {
            var item = MainTab.FirstOrDefault(t => t.OwnerControl.ViewModel is T);
            if (item == null)
            {
                return -1;
            }
            return MainTab.IndexOf(item);
        }

    }
}
