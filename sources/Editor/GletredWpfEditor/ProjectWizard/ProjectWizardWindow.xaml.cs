// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using System;

namespace GletredWpfEditor.ProjectWizard
{
    /// <summary>
    /// ProjectWizardWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ProjectWizardWindow : IProjectWizardWindow
    {
        public ProjectWizardWindowViewModel ViewModel => DataContext as ProjectWizardWindowViewModel ?? throw new InvalidOperationException();

        public ProjectWizardWindow()
        {
            InitializeComponent();
        }


        public void ShowWindow()
        {
            ShowDialog();
        }
    }
}
