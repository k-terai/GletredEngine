// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using GletredEdShare.CoreModule;
using GletredEdShare.WindowModule;
using GletredWpfEditor.AssetBrowser;
using GletredWpfEditor.LogViewer;


namespace GletredWpfEditor.Main
{
    public class MainWindowViewModel : WindowViewModel
    {

        private ObservableCollection<DockingWindowViewModel> _dockingWindows = null!;

        #region MainTabWindow

        private int _selectTabIndex;
        private bool _enableAssetBrowser;
        private bool _enableLogViewer;

        public bool EnableAssetBrowser
        {
            get => _enableAssetBrowser;
            set
            {
                _enableAssetBrowser = value;
                switch (value)
                {
                    case true when DockingWindows.Count(t => t.OwnerControl is IAssetBrowserControl) == 0:
                        DockingWindows.Add(new DockingWindowViewModel()
                        {
                            Name = Resources.AssetBrowser,
                            IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_AssetBrowser),
                            OwnerControl = EditorManager.CreateAssetBrowserControl()
                        });
                        SelectTabIndex = DockingWindows.Count - 1;
                        break;
                    case false when DockingWindows.Count(t => t.OwnerControl is IAssetBrowserControl) != 0:
                        DockingWindows.Remove(DockingWindows.First(t => t.OwnerControl is IAssetBrowserControl));
                        break;
                }
                NotifyPropertyChanged();
            }
        }

        public bool EnableLogViewer
        {
            get => _enableLogViewer;
            set
            {
                _enableLogViewer = value;
                switch (value)
                {
                    case true when DockingWindows.Count(t => t.OwnerControl is ILogViewerControl) == 0:
                        DockingWindows.Add(new DockingWindowViewModel()
                        {
                            Name = Resources.LogViewer,
                            IconSource = ResourceService.Current.GetFluentIconUri(Resources.Icon_LogViewer),
                            OwnerControl = EditorManager.CreateLogViewerControl()
                        });
                        break;
                    case false when DockingWindows.Count(t => t.OwnerControl is ILogViewerControl) != 0:
                        DockingWindows.Remove(DockingWindows.First(t => t.OwnerControl is ILogViewerControl));
                        break;
                }
                NotifyPropertyChanged();
            }
        }

        public int SelectTabIndex
        {
            get => _selectTabIndex;
            set
            {
                _selectTabIndex = value;
                NotifyPropertyChanged();
            }
        }

        private void InitializeTab()
        {
            EnableAssetBrowser = true;
            EnableLogViewer = true;
        }

        #endregion

        #region MainWindow

        private DockingWindowViewModel _activeContent = null!;
        private ObservableCollection<DockingWindowViewModel> _anchorables = null!;
        private ObservableCollection<DockingWindowViewModel> _documents = null!;
        private ObservableCollection<WindowItemViewModel> _toolbarElements = null!;

        public ObservableCollection<DockingWindowViewModel> Anchorables { get => _anchorables; set { _anchorables = value; NotifyPropertyChanged(); } }

        public ObservableCollection<DockingWindowViewModel> Documents { get => _documents; set { _documents = value; NotifyPropertyChanged(); } }

        public DelegateCommand OpenLogViewerCommand { get; private set; } = null!;

        public DelegateCommand OpenAssetBrowserCommand { get; private set; } = null!;

        public DelegateCommand OpenViewportCommand { get; private set; } = null!;

        public DelegateCommand ChangeToolbarCommand { get; private set; } = null!;

        public DockingWindowViewModel ActiveContent { get => _activeContent; set { _activeContent = value; NotifyPropertyChanged(); } }


        public ObservableCollection<WindowItemViewModel> ToolbarElements
        {
            get => _toolbarElements;
            set
            {
                _toolbarElements = value;
                NotifyPropertyChanged();
            }
        }


        private void OpenToolCommand<T>(DockingWindowViewModel.DockingType dockingType)
            where T : DockingWindowViewModel, new()
        {
            var tool = DockingWindows.FirstOrDefault(t => t is T);

            if (tool == null)
            {
                tool = new T();
                tool.Type = dockingType;

                switch (tool.Type)
                {
                    case DockingWindowViewModel.DockingType.Anchorable:
                        {
                            tool.CloseCommand = new DelegateCommand(_ =>
                            {
                                DockingWindows.Remove(tool);
                                Anchorables.Remove(tool);
                            });
                            Anchorables.Add(tool);
                        }
                        break;
                    case DockingWindowViewModel.DockingType.Document:
                        {
                            tool.CloseCommand = new DelegateCommand(_ =>
                            {
                                DockingWindows.Remove(tool);
                                Documents.Remove(tool);
                            });
                            Documents.Add(tool);
                        }
                        break;
                }

                DockingWindows.Add(tool);
            }

            tool.IsSelected = true;
            ActiveContent = tool;
        }

        #endregion


        public IMainWindow MainWindow { get; set; } = null!;

        public ObservableCollection<DockingWindowViewModel> DockingWindows { get => _dockingWindows; set { _dockingWindows = value; NotifyPropertyChanged(); } }


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
            DockingWindows = new ObservableCollection<DockingWindowViewModel>();
            Anchorables = new ObservableCollection<DockingWindowViewModel>();
            Documents = new ObservableCollection<DockingWindowViewModel>();

            InitCommands();
            InitializeTab();
        }


        private void InitCommands()
        {
            OpenLogViewerCommand = new DelegateCommand(
                (_) =>
                {
                    OpenToolCommand<PanesTemplateSelector.LogViewerPaneViewModel>(DockingWindowViewModel.DockingType.Anchorable);
                }
            );

            OpenAssetBrowserCommand = new DelegateCommand(
                (_) =>
                {
                    OpenToolCommand<PanesTemplateSelector.AssetBrowserPaneViewModel>(DockingWindowViewModel.DockingType.Anchorable);
                }
            );


            OpenViewportCommand = new DelegateCommand(
                (_) =>
                {
                    OpenToolCommand<PanesTemplateSelector.ViewportPaneViewModel>(DockingWindowViewModel.DockingType.Document);
                }
            );

            ChangeToolbarCommand = new DelegateCommand(
                (p) =>
                {
                    var toolbarType = (ToolbarType)p!;

                    if (toolbarType == ToolbarType.Main)
                    {
                        ToolbarElements = new ObservableCollection<WindowItemViewModel>
                        {
                            new ButtonViewModel()
                            {
                                Width = 60,
                                Name = Resources.Save,
                                ToolTip = Resources.SaveToolTip,
                                ImageUri = ResourceService.Current.GetFluentIconUri(Resources.Icon_Save)
                            },
                            new ButtonViewModel()
                            {
                                Width = 110,
                                Name = Resources.AssetBrowser,
                                ToolTip = Resources.AseetBrowserToolTip,
                                ImageUri = ResourceService.Current.GetFluentIconUri(Resources
                                    .Icon_AssetBrowser)
                            }
                        };
                    }
                    else if (toolbarType == ToolbarType.LevelEditor)
                    {

                    }
                });
        }

    }
}
