// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System.Windows;
using GletredEdShare.CoreModule;
using GletredEdShare.ProjectModule;
using GletredEdShare.WindowModule;

namespace GletredWpfEditor.ProjectWizard
{
    public class ProjectWizardWindowViewModel : WindowViewModel
    {

        private ProjectViewModel _projectViewModel = null!;

        public DelegateCommand SelectLocationCommand { get; private set; } = null!;
        public DelegateCommand CancelCommand { get; set; } = null!;
        public ProjectViewModel ProjectViewModel { get => _projectViewModel; set { _projectViewModel = value; NotifyPropertyChanged(); } }

        public event System.Action OnCancel = null!;

        public ProjectWizardWindowViewModel()
        {
            Width = 650;
            MinWidth = 650;

            Height = 650;
            MinHeight = 650;
            Title = "Project Window";
            Style = WindowStyle.ToolWindow;
            State = WindowState.Normal;
            ResizeModeType = ResizeMode.NoResize;

            ProjectViewModel = new ProjectViewModel();
            InitCommands();
        }

        private void InitCommands()
        {
            ProjectViewModel.OnProjectCreated += p =>
            {
                EditorManager.Restart(p.DataPath);
            };

            SelectLocationCommand = new DelegateCommand(

                _ =>
                {
                    var path = EditorManager.CreateSelectExternalFolderWindow().ShowWindow(Resources.SelectProjectPath);
                    if (path != string.Empty)
                    {
                        ProjectViewModel.Location = path;
                    }
                }
            );

            CancelCommand = new DelegateCommand(

                _ =>
                {
                    OnCancel();
                }
            );
        }
    }
}
