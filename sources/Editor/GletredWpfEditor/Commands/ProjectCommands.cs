// Copyright (c) k-terai and Contributors
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.

using GletredEdShare.CoreModule;

namespace GletredWpfEditor.Commands
{
    public static partial class EditorCommand
    {
        public static DelegateCommand OpenProjectCommand { get; private set; } = null!;

        public static DelegateCommand OpenProjectWizardWindowCommand { get; private set; } = null!;

        private static void InitProjectCommands()
        {
            OpenProjectCommand = new DelegateCommand(

                _ =>
                {
                    var window = EditorManager.CreateSelectExternalFileWindow();
                    var path = window.ShowWindow("Please select open project file.", "Project File(*.project) |*.project", false);
                    if (path != null && path.Count != 0)
                    {
                        EditorManager.Restart(path[0]);
                    }
                }
                ,
                _ => true);

            OpenProjectWizardWindowCommand = new DelegateCommand(

                _ =>
                {
                    var window = EditorManager.CreateProjectWizardWindow();
                    window.ShowWindow();
                }
                ,
                _ => true);

            AllCommands["OpenProjectCommand"] = OpenProjectCommand;
            AllCommands["OpenProjectWizardWindowCommand"] = OpenProjectWizardWindowCommand;
        }
    }
}