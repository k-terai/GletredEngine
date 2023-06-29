﻿// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.ObjectModel;
using GletredEdShare.WindowModule;


namespace GletredWpfEditor.Main
{
    public class MainWindowViewModel : WindowViewModel
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
            InitMainTab();
        }


        private void InitCommands()
        {

        }

        private void InitMainTab()
        {
            MainTab = new ObservableCollection<DockingWindowViewModel>
            {
                new()
                {
                    Name = Resources.Portal,
                    IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_Portal),
                    OwnerControl = EditorManager.CreatePortalControl()
                }
            };
        }

    }
}
